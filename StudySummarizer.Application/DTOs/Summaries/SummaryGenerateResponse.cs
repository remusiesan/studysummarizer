namespace StudySummarizer.Application.DTOs.Summaries;

public class SummaryGenerateResponse
{
    public string Message { get; set; } = "Summarization started";
    public string DocumentId { get; set; } = string.Empty;
}
