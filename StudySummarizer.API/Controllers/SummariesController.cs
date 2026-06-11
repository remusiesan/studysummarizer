using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySummarizer.Application.DTOs;
using StudySummarizer.Application.DTOs.Summaries;
using StudySummarizer.Application.Interfaces;

namespace StudySummarizer.API.Controllers;

[ApiController]
[Route("api/documents")]
[Authorize]
public class SummariesController : ControllerBase
{
    private readonly ISummaryService _summaryService;
    private readonly ILogger<SummariesController> _logger;

    public SummariesController(ISummaryService summaryService, ILogger<SummariesController> logger)
    {
        _summaryService = summaryService;
        _logger = logger;
    }

    [HttpPost("{documentId}/summarize")]
    public async Task<IActionResult> GenerateSummary(string documentId, [FromBody] SummaryGenerateRequest request)
    {
        var result = await _summaryService.GenerateSummaryAsync(documentId, request);
        return Created($"/api/documents/{documentId}/summary", ApiResponse<SummaryGenerateResponse>.SuccessResponse(result, "Summary generation started"));
    }

    [AllowAnonymous]
    [HttpGet("{documentId}/summary")]
    public async Task<IActionResult> GetSummary(string documentId)
    {
        var summary = await _summaryService.GetSummaryAsync(documentId);
        return Ok(ApiResponse<SummaryDetailResponse>.SuccessResponse(summary, "Summary retrieved successfully"));
    }

    [HttpPatch("{documentId}/summary")]
    public async Task<IActionResult> UpdateSummary(string documentId, [FromBody] SummaryUpdateRequest request)
    {
        var result = await _summaryService.UpdateSummaryAsync(documentId, request);
        return Ok(ApiResponse<SummaryUpdateResponse>.SuccessResponse(result, "Summary updated successfully"));
    }
}
