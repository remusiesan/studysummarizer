namespace StudySummarizer.Services;

public interface IIdGeneratorService
{
    string GenerateUserId();
    string GenerateDocumentId();
    string GenerateSummaryId();
}

public class IdGeneratorService : IIdGeneratorService
{
    private int _userCounter = 0;
    private int _documentCounter = 0;
    private int _summaryCounter = 0;

    public string GenerateUserId()
    {
        _userCounter++;
        return $"U{_userCounter}";
    }

    public string GenerateDocumentId()
    {
        _documentCounter++;
        return $"D{_documentCounter}";
    }

    public string GenerateSummaryId()
    {
        _summaryCounter++;
        return $"S{_summaryCounter}";
    }
}
