using StudySummarizer.Application.DTOs.Auth;

namespace StudySummarizer.Application.Services.Interfaces;

public interface IAuthService
{
    Task<UserRegisterResponse> RegisterAsync(UserRegisterRequest request);
    Task<UserLoginResponse> LoginAsync(UserLoginRequest request);
    Task<UserProfileResponse> GetProfileAsync(string userId);
}
