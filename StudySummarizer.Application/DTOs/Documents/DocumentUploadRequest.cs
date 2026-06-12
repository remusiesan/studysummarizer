using StudySummarizer.Application.Services.Interfaces;

namespace StudySummarizer.Application.DTOs.Documents;

public class DocumentUploadRequest
{
    public string Title { get; set; } = string.Empty;
    public IFileUpload File { get; set; } = null!;
}
