using FluentValidation;
using Microsoft.Extensions.Logging;
using StudySummarizer.Application.DTOs.Summaries;
using StudySummarizer.Application.Repositories.Interfaces;
using StudySummarizer.Application.Services.Interfaces;
using StudySummarizer.Domain.Constants;
using StudySummarizer.Domain.Entities;
using StudySummarizer.Domain.Exceptions;

namespace StudySummarizer.Application.Services;

public class SummaryService : ISummaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SummaryService> _logger;
    private readonly IIdGeneratorService _idGenerator;
    private readonly IValidator<SummaryGenerateRequest> _generateValidator;
    private readonly IValidator<SummaryUpdateRequest> _updateValidator;

    public SummaryService(
        IUnitOfWork unitOfWork,
        ILogger<SummaryService> logger,
        IIdGeneratorService idGenerator,
        IValidator<SummaryGenerateRequest> generateValidator,
        IValidator<SummaryUpdateRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _idGenerator = idGenerator;
        _generateValidator = generateValidator;
        _updateValidator = updateValidator;
    }

    public async Task<SummaryGenerateResponse> GenerateSummaryAsync(string documentId, SummaryGenerateRequest request)
    {
        await _generateValidator.ValidateAndThrowAsync(request);

        var document = _unitOfWork.Documents.Get(d => d.Id == documentId);
        if (document == null)
            throw new NotFoundException("Document not found");

        var existingSummary = _unitOfWork.Summaries.Get(s => s.DocumentId == documentId);
        if (existingSummary != null)
            throw new StudySummarizer.Domain.Exceptions.ValidationException("A summary already exists for this document");

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

        _unitOfWork.Summaries.Add(summary);

        document.Status = DocumentStatusValues.Summarizing;
        _unitOfWork.Documents.Update(document);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Summarization started for document {DocumentId}", documentId);

        return new SummaryGenerateResponse
        {
            Message = "Summarization started",
            DocumentId = documentId
        };
    }

    public async Task<SummaryDetailResponse> GetSummaryAsync(string documentId)
    {
        var document = _unitOfWork.Documents.Get(d => d.Id == documentId);
        if (document == null)
            throw new NotFoundException("Document not found");

        var summary = _unitOfWork.Summaries.Get(s => s.DocumentId == documentId);
        if (summary == null)
            throw new NotFoundException("No summary found for this document");

        return await Task.FromResult(new SummaryDetailResponse
        {
            DocumentId = summary.DocumentId,
            Title = summary.Title,
            Summary = summary.Content,
            GeneratedAt = summary.GeneratedAt
        });
    }

    public async Task<SummaryUpdateResponse> UpdateSummaryAsync(string documentId, SummaryUpdateRequest request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var document = _unitOfWork.Documents.Get(d => d.Id == documentId);
        if (document == null)
            throw new NotFoundException("Document not found");

        var summary = _unitOfWork.Summaries.Get(s => s.DocumentId == documentId);
        if (summary == null)
            throw new NotFoundException("No summary found for this document");

        summary.SummaryType = request.SummaryType;
        summary.Content = $"[Regenerating {request.SummaryType} summary for {document.Title}]";
        summary.UpdatedAt = DateTime.UtcNow;

        document.Status = DocumentStatusValues.Summarizing;

        _unitOfWork.Summaries.Update(summary);
        _unitOfWork.Documents.Update(document);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Summary for document {DocumentId} regenerated", documentId);

        return new SummaryUpdateResponse
        {
            Message = "Summary regenerated successfully"
        };
    }
}
