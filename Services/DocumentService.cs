using Microsoft.EntityFrameworkCore;
using StudySummarizer.Data;
using StudySummarizer.DTOs.Auth;
using StudySummarizer.DTOs.Documents;
using StudySummarizer.Exceptions;
using StudySummarizer.Models;

namespace StudySummarizer.Services;

public interface IDocumentService
{
    Task<DocumentUploadResponse> UploadDocumentAsync(DocumentUploadRequest request, string userId);
    Task<List<DocumentListItemResponse>> GetAllDocumentsAsync();
    Task<DocumentDetailResponse> GetDocumentAsync(string documentId);
    Task<byte[]> DownloadDocumentAsync(string documentId);
    Task<DocumentDeleteResponse> DeleteDocumentAsync(string documentId);
    Task<List<UserDocumentListResponse>> GetUserDocumentsAsync(string userId);
}

public class DocumentService : IDocumentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<DocumentService> _logger;
    private readonly IIdGeneratorService _idGenerator;

    public DocumentService(AppDbContext context, ILogger<DocumentService> logger, IIdGeneratorService idGenerator)
    {
        _context = context;
        _logger = logger;
        _idGenerator = idGenerator;
    }

    public async Task<DocumentUploadResponse> UploadDocumentAsync(DocumentUploadRequest request, string userId)
    {
        if (request.File == null || request.File.Length == 0)
            throw new ValidationException("File is required");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ValidationException("Title is required");

        var documentId = _idGenerator.GenerateDocumentId();

        // Extract file type from file extension
        var fileExtension = Path.GetExtension(request.File.FileName).TrimStart('.');
        if (string.IsNullOrWhiteSpace(fileExtension))
            fileExtension = "unknown";

        // Create file path
        var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Directory.Exists(uploadsDirectory))
            Directory.CreateDirectory(uploadsDirectory);

        var fileName = $"{documentId}_{request.File.FileName}";
        var filePath = Path.Combine("uploads", fileName);
        var fullPath = Path.Combine(uploadsDirectory, fileName);

        // Save file to disk
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var document = new Document
        {
            Id = documentId,
            UserId = userId,
            Title = request.Title,
            FileType = fileExtension,
            FilePath = filePath,
            Status = DocumentStatusValues.Pending,
            UploadDate = DateTime.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Document {DocumentId} uploaded by user {UserId}", documentId, userId);

        return new DocumentUploadResponse
        {
            Message = "Document uploaded successfully",
            Id = documentId
        };
    }

    public async Task<List<DocumentListItemResponse>> GetAllDocumentsAsync()
    {
        var documents = await _context.Documents
            .Select(d => new DocumentListItemResponse
            {
                Id = d.Id,
                Title = d.Title,
                FileType = d.FileType,
                Status = d.Status
            })
            .ToListAsync();

        return documents;
    }

    public async Task<DocumentDetailResponse> GetDocumentAsync(string documentId)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new NotFoundException("Document not found");

        return new DocumentDetailResponse
        {
            Id = document.Id,
            Title = document.Title,
            FileType = document.FileType,
            Status = document.Status,
            UploadDate = document.UploadDate
        };
    }

    public async Task<byte[]> DownloadDocumentAsync(string documentId)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new NotFoundException("Document not found");

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), document.FilePath);
        if (!File.Exists(filePath))
            throw new NotFoundException("Document file not found");

        _logger.LogInformation("Document {DocumentId} downloaded", documentId);

        return await File.ReadAllBytesAsync(filePath);
    }

    public async Task<DocumentDeleteResponse> DeleteDocumentAsync(string documentId)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new NotFoundException("Document not found");

        _context.Documents.Remove(document);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Document {DocumentId} deleted", documentId);

        return new DocumentDeleteResponse
        {
            Message = "Document deleted successfully"
        };
    }

    public async Task<List<UserDocumentListResponse>> GetUserDocumentsAsync(string userId)
    {
        var documents = await _context.Documents
            .Where(d => d.UserId == userId)
            .Select(d => new UserDocumentListResponse
            {
                DocumentId = d.Id,
                Title = d.Title,
                Status = d.Status
            })
            .ToListAsync();

        return documents;
    }
}
