namespace StudySummarizer.Application.Services.Interfaces;

public interface IIdGeneratorService
{
    string GenerateUserId();
    string GenerateDocumentId();
    string GenerateSummaryId();
}
