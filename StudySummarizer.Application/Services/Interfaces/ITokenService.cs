namespace StudySummarizer.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(string userId, string username, string email);
}
