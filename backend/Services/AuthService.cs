using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudySummarizer.Data;
using StudySummarizer.DTOs.Auth;
using StudySummarizer.Exceptions;
using StudySummarizer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudySummarizer.Services;

public interface IAuthService
{
    Task<UserRegisterResponse> RegisterAsync(UserRegisterRequest request);
    Task<UserLoginResponse> LoginAsync(UserLoginRequest request);
    Task<UserProfileResponse> GetProfileAsync(string userId);
}

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;
    private readonly IIdGeneratorService _idGenerator;

    public AuthService(AppDbContext context, IConfiguration config, ILogger<AuthService> logger, IIdGeneratorService idGenerator)
    {
        _context = context;
        _config = config;
        _logger = logger;
        _idGenerator = idGenerator;
    }

    public async Task<UserRegisterResponse> RegisterAsync(UserRegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ValidationException("Email is required");

        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new ValidationException("Email already exists");

        var userId = _idGenerator.GenerateUserId();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var username = request.Email.Split('@')[0];

        var user = new User
        {
            Id = userId,
            Username = username,
            Email = request.Email,
            PasswordHash = passwordHash,
            IsActive = true,
            RegisteredAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User {Username} registered successfully", user.Username);

        return new UserRegisterResponse
        {
            Message = "User registered successfully",
            UserId = userId,
            Token = GenerateJwtToken(user)
        };
    }

    public async Task<UserLoginResponse> LoginAsync(UserLoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid email or password");

        user.LastLogin = DateTime.UtcNow;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User {Username} logged in successfully", user.Username);

        return new UserLoginResponse
        {
            Message = "Login successful",
            Token = GenerateJwtToken(user),
            UserId = user.Id
        };
    }

    public async Task<UserProfileResponse> GetProfileAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");

        return new UserProfileResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            RegisteredAt = user.RegisteredAt
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSecret = _config[Constants.Jwt.ConfigKeySecret] ?? throw new InvalidOperationException("JWT secret not configured");
        var jwtIssuer = _config[Constants.Jwt.ConfigKeyIssuer] ?? Constants.Jwt.DefaultIssuer;
        var jwtAudience = _config[Constants.Jwt.ConfigKeyAudience] ?? Constants.Jwt.DefaultAudience;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
