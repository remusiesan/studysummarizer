using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySummarizer.DTOs;
using StudySummarizer.Application.Extensions;
using StudySummarizer.DTOs.Documents;
using StudySummarizer.Services;

namespace StudySummarizer.Controllers;

[ApiController]
[Route("api/documents")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentsController> _logger;

    public DocumentsController(IDocumentService documentService, ILogger<DocumentsController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UploadDocument([FromForm] string title, IFormFile file)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var request = new DocumentUploadRequest
        {
            Title = title,
            File = file
        };

        var result = await _documentService.UploadDocumentAsync(request, userId);
        return Created($"/api/documents/{result.Id}", ApiResponse<DocumentUploadResponse>.SuccessResponse(result, "Document uploaded successfully"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDocuments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var documents = await _documentService.GetAllDocumentsAsync();
        var paginated = documents.GetPaginated(pageNumber, pageSize);
        return Ok(ApiResponse<PaginatedResponse<DocumentListItemResponse>>.SuccessResponse(paginated, "Documents retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocument(string id)
    {
        var document = await _documentService.GetDocumentAsync(id);
        return Ok(ApiResponse<DocumentDetailResponse>.SuccessResponse(document, "Document retrieved successfully"));
    }

    [AllowAnonymous]
    [HttpGet("{id}/file")]
    public async Task<IActionResult> DownloadDocument(string id)
    {
        var stream = await _documentService.DownloadDocumentAsync(id);
        var document = await _documentService.GetDocumentAsync(id);
        var fileName = $"{document.Title}.{document.FileType}";
        return File(stream, "application/octet-stream", fileName, enableRangeProcessing: true);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocument(string id)
    {
        var result = await _documentService.DeleteDocumentAsync(id);
        return Ok(ApiResponse<DocumentDeleteResponse>.SuccessResponse(result, "Document deleted successfully"));
    }
}
