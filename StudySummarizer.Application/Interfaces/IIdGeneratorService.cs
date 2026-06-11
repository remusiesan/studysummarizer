namespace StudySummarizer.Application.Interfaces;

public interface IIdGeneratorService
{
    string GenerateUserId();
    string GenerateDocumentId();
    string GenerateSummaryId();
}
