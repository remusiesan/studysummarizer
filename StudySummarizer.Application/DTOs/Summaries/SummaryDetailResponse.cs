namespace StudySummarizer.Application.DTOs.Summaries;

public class SummaryDetailResponse
{
    public string DocumentId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}
