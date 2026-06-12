using StudySummarizer.Application.DTOs.Summaries;

namespace StudySummarizer.Application.Services.Interfaces;

public interface ISummaryService
{
    Task<SummaryGenerateResponse> GenerateSummaryAsync(string documentId, SummaryGenerateRequest request);
    Task<SummaryDetailResponse> GetSummaryAsync(string documentId);
    Task<SummaryUpdateResponse> UpdateSummaryAsync(string documentId, SummaryUpdateRequest request);
}
