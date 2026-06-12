namespace StudySummarizer.Application.DTOs.Auth;

public class UserLoginResponse
{
    public string Message { get; set; } = "Login successful";
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
