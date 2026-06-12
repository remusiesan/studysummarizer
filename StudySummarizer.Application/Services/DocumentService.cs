using FluentValidation;
using Microsoft.Extensions.Logging;
using StudySummarizer.Application.DTOs.Auth;
using StudySummarizer.Application.DTOs.Documents;
using StudySummarizer.Application.Repositories.Interfaces;
using StudySummarizer.Application.Services.Interfaces;
using StudySummarizer.Domain.Constants;
using StudySummarizer.Domain.Entities;
using StudySummarizer.Domain.Exceptions;

namespace StudySummarizer.Application.Services;

public class DocumentService : IDocumentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DocumentService> _logger;
    private readonly IIdGeneratorService _idGenerator;
    private readonly IValidator<DocumentUploadRequest> _uploadValidator;

    public DocumentService(
        IUnitOfWork unitOfWork,
        ILogger<DocumentService> logger,
        IIdGeneratorService idGenerator,
        IValidator<DocumentUploadRequest> uploadValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _idGenerator = idGenerator;
        _uploadValidator = uploadValidator;
    }

    public async Task<DocumentUploadResponse> UploadDocumentAsync(DocumentUploadRequest request, string userId)
    {
        await _uploadValidator.ValidateAndThrowAsync(request);

        var fileName = request.File.FileName ?? "file";
        var fileExtension = Path.GetExtension(fileName).TrimStart('.').ToLower();
        var documentId = _idGenerator.GenerateDocumentId();

        try
        {
            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                await request.File.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            var document = new Document
            {
                Id = documentId,
                UserId = userId,
                Title = request.Title,
                FileType = fileExtension,
                FileName = fileName,
                FileSize = request.File.Length,
                FileContent = fileContent,
                Status = DocumentStatusValues.Pending,
                UploadDate = DateTime.UtcNow
            };

            _unitOfWork.Documents.Add(document);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation(
                "Document {DocumentId} uploaded by user {UserId}. File: {FileName}, Size: {FileSize} bytes, Extension: {Extension}",
                documentId, userId, fileName, request.File.Length, fileExtension);

            return new DocumentUploadResponse
            {
                Message = "Document uploaded successfully",
                Id = documentId
            };
        }
        catch (Exception ex) when (ex is not FluentValidation.ValidationException)
        {
            _logger.LogError(ex, "Error while uploading document {DocumentId}", documentId);
            throw new StudySummarizer.Domain.Exceptions.ValidationException("Failed to save the document. Please try again or contact support if the issue persists.");
        }
    }

    public async Task<List<DocumentListItemResponse>> GetAllDocumentsAsync()
    {
        var documents = _unitOfWork.Documents.GetAll()
            .Select(d => new DocumentListItemResponse
            {
                Id = d.Id,
                Title = d.Title,
                FileType = d.FileType,
                Status = d.Status,
                UploadedAt = d.UploadDate,
                FileSize = d.FileSize
            })
            .ToList();

        return await Task.FromResult(documents);
    }

    public async Task<DocumentDetailResponse> GetDocumentAsync(string documentId)
    {
        var document = _unitOfWork.Documents.Get(d => d.Id == documentId);
        if (document == null)
            throw new NotFoundException($"Document with ID '{documentId}' not found.");

        return await Task.FromResult(new DocumentDetailResponse
        {
            Id = document.Id,
            Title = document.Title,
            FileType = document.FileType,
            Status = document.Status,
            UploadedAt = document.UploadDate,
            FileSize = document.FileSize
        });
    }

    public async Task<Stream> DownloadDocumentAsync(string documentId)
    {
        var document = _unitOfWork.Documents.Get(d => d.Id == documentId);
        if (document == null)
            throw new NotFoundException($"Document with ID '{documentId}' not found.");

        if (document.FileContent == null || document.FileContent.Length == 0)
        {
            _logger.LogWarning("Document file content is empty for document {DocumentId}", documentId);
            throw new NotFoundException(
                $"The document file for '{document.Title}' is empty or corrupted. Please re-upload the document.");
        }

        _logger.LogInformation("Document {DocumentId} downloaded by user {UserId}", documentId, document.UserId);

        var memoryStream = new MemoryStream(document.FileContent, writable: false);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return await Task.FromResult<Stream>(memoryStream);
    }

    public async Task<DocumentDeleteResponse> DeleteDocumentAsync(string documentId)
    {
        var document = _unitOfWork.Documents.Get(d => d.Id == documentId);
        if (document == null)
            throw new NotFoundException($"Document with ID '{documentId}' not found.");

        try
        {
            _unitOfWork.Documents.Delete(document);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Document {DocumentId} deleted successfully", documentId);

            return new DocumentDeleteResponse
            {
                Message = "Document deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting document {DocumentId}", documentId);
            throw new StudySummarizer.Domain.Exceptions.ValidationException("An error occurred while deleting the document. Please try again.");
        }
    }

    public async Task<List<UserDocumentListResponse>> GetUserDocumentsAsync(string userId)
    {
        var documents = _unitOfWork.Documents.Find(d => d.UserId == userId)
            .Select(d => new UserDocumentListResponse
            {
                DocumentId = d.Id,
                Title = d.Title,
                Status = d.Status
            })
            .ToList();

        return await Task.FromResult(documents);
    }
}
