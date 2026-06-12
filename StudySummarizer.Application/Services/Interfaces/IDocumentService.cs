using StudySummarizer.Application.DTOs.Auth;
using StudySummarizer.Application.DTOs.Documents;

namespace StudySummarizer.Application.Services.Interfaces;

public interface IDocumentService
{
    Task<DocumentUploadResponse> UploadDocumentAsync(DocumentUploadRequest request, string userId);
    Task<List<DocumentListItemResponse>> GetAllDocumentsAsync();
    Task<DocumentDetailResponse> GetDocumentAsync(string documentId);
    Task<Stream> DownloadDocumentAsync(string documentId);
    Task<DocumentDeleteResponse> DeleteDocumentAsync(string documentId);
    Task<List<UserDocumentListResponse>> GetUserDocumentsAsync(string userId);
}
