using StudySummarizer.Domain.Constants;

namespace StudySummarizer.Domain.Entities;

public class Document
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public byte[] FileContent { get; set; } = [];
    public string Status { get; set; } = DocumentStatusValues.Pending;
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public User? User { get; set; }
    public List<Summary> Summaries { get; set; } = [];
}
