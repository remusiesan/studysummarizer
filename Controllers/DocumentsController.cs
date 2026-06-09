using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        var response = await _documentService.UploadDocumentAsync(request, userId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDocuments()
    {
        var documents = await _documentService.GetAllDocumentsAsync();
        return Ok(documents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocument(string id)
    {
        var document = await _documentService.GetDocumentAsync(id);
        return Ok(document);
    }

    [HttpGet("{id}/file")]
    public async Task<IActionResult> DownloadDocument(string id)
    {
        var fileContent = await _documentService.DownloadDocumentAsync(id);
        var document = await _documentService.GetDocumentAsync(id);
        return File(fileContent, "application/octet-stream", $"{document.Title}.{document.FileType}");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocument(string id)
    {
        var response = await _documentService.DeleteDocumentAsync(id);
        return Ok(response);
    }
}
