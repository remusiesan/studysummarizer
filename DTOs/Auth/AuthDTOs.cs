namespace StudySummarizer.DTOs.Auth;

public class UserRegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserRegisterResponse
{
    public string Message { get; set; } = "User registered successfully";
    public string UserId { get; set; } = string.Empty;
}

public class UserLoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserLoginResponse
{
    public string Message { get; set; } = "Login successful";
    public string Token { get; set; } = string.Empty;
}

public class UserProfileResponse
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
}

public class UserDocumentListResponse
{
    public string DocumentId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
