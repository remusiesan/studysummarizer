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
    Task<Stream> DownloadDocumentAsync(string documentId);
    Task<DocumentDeleteResponse> DeleteDocumentAsync(string documentId);
    Task<List<UserDocumentListResponse>> GetUserDocumentsAsync(string userId);
}

public class DocumentService : IDocumentService
{
    // Configuration constants
    private const long MaxFileSize = 20 * 1024 * 1024; // 20 MB

    private static readonly string[] AllowedExtensions =
    {
        "pdf", "doc", "docx", "txt", "xls", "xlsx",
        "ppt", "pptx", "png", "jpg", "jpeg", "gif"
    };

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
        // Validate file exists
        if (request.File == null || request.File.Length == 0)
            throw new ValidationException("File is required. Please select a file to upload.");

        // Validate title
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ValidationException("Document title is required.");

        // Validate title length
        if (request.Title.Length > 255)
            throw new ValidationException("Document title cannot exceed 255 characters.");

        // Validate file size
        if (request.File.Length > MaxFileSize)
            throw new ValidationException(
                $"File size exceeds the maximum limit of 20MB. Your file is {FormatFileSize(request.File.Length)}.");

        // Extract and validate file extension
        var fileName = request.File.FileName ?? "file";
        var fileExtension = Path.GetExtension(fileName).TrimStart('.').ToLower();

        if (string.IsNullOrWhiteSpace(fileExtension))
            throw new ValidationException("File must have a valid extension (e.g., .pdf, .docx, .txt).");

        if (!AllowedExtensions.Contains(fileExtension))
            throw new ValidationException(
                $"File format '{fileExtension.ToUpper()}' is not allowed. " +
                $"Supported formats: {string.Join(", ", AllowedExtensions.Select(e => e.ToUpper()))}");

        var documentId = _idGenerator.GenerateDocumentId();

        try
        {
            // Read file content into memory
            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                await request.File.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            // Create document with file content stored in database
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

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Document {DocumentId} uploaded by user {UserId}. File: {FileName}, Size: {FileSize} bytes, Extension: {Extension}",
                documentId, userId, fileName, request.File.Length, fileExtension);

            return new DocumentUploadResponse
            {
                Message = "Document uploaded successfully",
                Id = documentId
            };
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database error while uploading document {DocumentId}", documentId);
            throw new ValidationException("Failed to save the document. Please try again or contact support if the issue persists.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while uploading document {DocumentId}", documentId);
            throw new ValidationException("An unexpected error occurred while uploading the file. Please try again.");
        }
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
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ValidationException("Document ID is required.");

        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new NotFoundException($"Document with ID '{documentId}' not found.");

        return new DocumentDetailResponse
        {
            Id = document.Id,
            Title = document.Title,
            FileType = document.FileType,
            Status = document.Status,
            UploadDate = document.UploadDate
        };
    }

    public async Task<Stream> DownloadDocumentAsync(string documentId)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ValidationException("Document ID is required.");

        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new NotFoundException($"Document with ID '{documentId}' not found.");

        if (document.FileContent == null || document.FileContent.Length == 0)
        {
            _logger.LogWarning("Document file content is empty for document {DocumentId}", documentId);
            throw new NotFoundException(
                $"The document file for '{document.Title}' is empty or corrupted. Please re-upload the document.");
        }

        _logger.LogInformation("Document {DocumentId} downloaded by user {UserId}", documentId, document.UserId);

        // Return memory stream with file content from database
        var memoryStream = new MemoryStream(document.FileContent, writable: false);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public async Task<DocumentDeleteResponse> DeleteDocumentAsync(string documentId)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ValidationException("Document ID is required.");

        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new NotFoundException($"Document with ID '{documentId}' not found.");

        try
        {
            // Delete from database (file content is automatically deleted)
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Document {DocumentId} deleted successfully", documentId);

            return new DocumentDeleteResponse
            {
                Message = "Document deleted successfully"
            };
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database error while deleting document {DocumentId}", documentId);
            throw new ValidationException("Unable to delete the document. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting document {DocumentId}", documentId);
            throw new ValidationException("An error occurred while deleting the document. Please try again.");
        }
    }

    public async Task<List<UserDocumentListResponse>> GetUserDocumentsAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ValidationException("User ID is required.");

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

    /// <summary>
    /// Formats file size in human-readable format (KB, MB, GB)
    /// </summary>
    private static string FormatFileSize(long bytes)
    {
        var units = new[] { "B", "KB", "MB", "GB" };
        double size = bytes;
        int unitIndex = 0;

        while (size >= 1024 && unitIndex < units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        return $"{size:F2} {units[unitIndex]}";
    }
}
