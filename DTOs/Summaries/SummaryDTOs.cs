namespace StudySummarizer.DTOs.Summaries;

public class SummaryGenerateRequest
{
    public string SummaryType { get; set; } = string.Empty;
}

public class SummaryGenerateResponse
{
    public string Message { get; set; } = "Summarization started";
    public string DocumentId { get; set; } = string.Empty;
}

public class SummaryDetailResponse
{
    public string DocumentId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}

public class SummaryUpdateRequest
{
    public string SummaryType { get; set; } = string.Empty;
}

public class SummaryUpdateResponse
{
    public string Message { get; set; } = "Summary regenerated successfully";
}
