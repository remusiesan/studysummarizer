using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySummarizer.DTOs.Summaries;
using StudySummarizer.Services;

namespace StudySummarizer.Controllers;

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
        var response = await _summaryService.GenerateSummaryAsync(documentId, request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{documentId}/summary")]
    public async Task<IActionResult> GetSummary(string documentId)
    {
        var summary = await _summaryService.GetSummaryAsync(documentId);
        return Ok(summary);
    }

    [HttpPatch("{documentId}/summary")]
    public async Task<IActionResult> UpdateSummary(string documentId, [FromBody] SummaryUpdateRequest request)
    {
        var response = await _summaryService.UpdateSummaryAsync(documentId, request);
        return Ok(response);
    }
}
