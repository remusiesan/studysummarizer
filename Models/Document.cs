namespace StudySummarizer.Models;

public class Document
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Status { get; set; } = DocumentStatusValues.Pending;
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public User? User { get; set; }
    public List<Summary> Summaries { get; set; } = [];
}

public static class DocumentStatusValues
{
    public const string Pending = "pending";
    public const string Summarizing = "summarizing";
    public const string Summarized = "summarized";
}
