using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySummarizer.DTOs;
using StudySummarizer.DTOs.Auth;
using StudySummarizer.Services;

namespace StudySummarizer.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IDocumentService _documentService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, IDocumentService documentService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _documentService = documentService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(ApiResponse<UserRegisterResponse>.SuccessResponse(result, "User registered successfully"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponse<UserLoginResponse>.SuccessResponse(result, "Login successful"));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var profile = await _authService.GetProfileAsync(userId);
        return Ok(ApiResponse<UserProfileResponse>.SuccessResponse(profile, "Profile retrieved successfully"));
    }

    [Authorize]
    [HttpGet("{userId}/documents")]
    public async Task<IActionResult> GetUserDocuments(string userId)
    {
        var documents = await _documentService.GetUserDocumentsAsync(userId);
        return Ok(ApiResponse<List<UserDocumentListResponse>>.SuccessResponse(documents, "Documents retrieved successfully"));
    }
}

