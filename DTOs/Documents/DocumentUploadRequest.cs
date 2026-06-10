namespace StudySummarizer.DTOs.Documents;

public class DocumentUploadRequest
{
    public string Title { get; set; } = string.Empty;
    public IFormFile File { get; set; } = null!;
}

public class DocumentUploadResponse
{
    public string Message { get; set; } = "Document uploaded successfully";
    public string Id { get; set; } = string.Empty;
}

public class DocumentListItemResponse
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public long FileSize { get; set; }
}

public class DocumentDetailResponse
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public long FileSize { get; set; }
}

public class DocumentDeleteResponse
{
    public string Message { get; set; } = "Document deleted successfully";
}
