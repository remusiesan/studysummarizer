namespace StudySummarizer.Application.DTOs.Auth;

public class UserRegisterResponse
{
    public string Message { get; set; } = "User registered successfully";
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
