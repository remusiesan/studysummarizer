using Microsoft.EntityFrameworkCore;
using StudySummarizer.Data;
using StudySummarizer.DTOs.Summaries;
using StudySummarizer.Exceptions;
using StudySummarizer.Models;

namespace StudySummarizer.Services;

public interface ISummaryService
{
    Task<SummaryGenerateResponse> GenerateSummaryAsync(string documentId, SummaryGenerateRequest request);
    Task<SummaryDetailResponse> GetSummaryAsync(string documentId);
    Task<SummaryUpdateResponse> UpdateSummaryAsync(string documentId, SummaryUpdateRequest request);
}

public class SummaryService : ISummaryService
{
    private readonly AppDbContext _context;
    private readonly ILogger<SummaryService> _logger;
    private readonly IIdGeneratorService _idGenerator;

    public SummaryService(AppDbContext context, ILogger<SummaryService> logger, IIdGeneratorService idGenerator)
    {
        _context = context;
        _logger = logger;
        _idGenerator = idGenerator;
    }

    public async Task<SummaryGenerateResponse> GenerateSummaryAsync(string documentId, SummaryGenerateRequest request)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new NotFoundException("Document not found");

        var existingSummary = await _context.Summaries.FirstOrDefaultAsync(s => s.DocumentId == documentId);
        if (existingSummary != null)
            throw new ValidationException("A summary already exists for this document");

        var summaryId = _idGenerator.GenerateSummaryId();

        var summary = new Summary
        {
            Id = summaryId,
            DocumentId = documentId,
            Title = document.Title,
            Content = $"[Generating {request.SummaryType} summary for {document.Title}]",
            SummaryType = request.SummaryType,
            GeneratedAt = DateTime.UtcNow
        };

        _context.Summaries.Add(summary);

        document.Status = DocumentStatusValues.Summarizing;
        _context.Documents.Update(document);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Summarization started for document {DocumentId}", documentId);

        return new SummaryGenerateResponse
        {
            Message = "Summarization started",
            DocumentId = documentId
        };
    }

    public async Task<SummaryDetailResponse> GetSummaryAsync(string documentId)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new NotFoundException("Document not found");

        var summary = await _context.Summaries
            .FirstOrDefaultAsync(s => s.DocumentId == documentId);

        if (summary == null)
            throw new NotFoundException("No summary found for this document");

        return new SummaryDetailResponse
        {
            DocumentId = summary.DocumentId,
            Title = summary.Title,
            Summary = summary.Content,
            GeneratedAt = summary.GeneratedAt
        };
    }

    public async Task<SummaryUpdateResponse> UpdateSummaryAsync(string documentId, SummaryUpdateRequest request)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new NotFoundException("Document not found");

        var summary = await _context.Summaries.FirstOrDefaultAsync(s => s.DocumentId == documentId);
        if (summary == null)
            throw new NotFoundException("No summary found for this document");

        summary.SummaryType = request.SummaryType;
        summary.Content = $"[Regenerating {request.SummaryType} summary for {document.Title}]";
        summary.UpdatedAt = DateTime.UtcNow;

        document.Status = DocumentStatusValues.Summarizing;

        _context.Summaries.Update(summary);
        _context.Documents.Update(document);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Summary for document {DocumentId} regenerated", documentId);

        return new SummaryUpdateResponse
        {
            Message = "Summary regenerated successfully"
        };
    }
}
