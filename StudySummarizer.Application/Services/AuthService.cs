using BCrypt.Net;
using Microsoft.Extensions.Logging;
using StudySummarizer.Application.DTOs.Auth;
using StudySummarizer.Application.Interfaces;
using StudySummarizer.Application.Repositories;
using StudySummarizer.Domain.Entities;
using StudySummarizer.Domain.Exceptions;

namespace StudySummarizer.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;
    private readonly IIdGeneratorService _idGenerator;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, ILogger<AuthService> logger, IIdGeneratorService idGenerator)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _logger = logger;
        _idGenerator = idGenerator;
    }

    public async Task<UserRegisterResponse> RegisterAsync(UserRegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ValidationException("Email is required");

        if (_unitOfWork.Users.Get(u => u.Email == request.Email) != null)
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

        _unitOfWork.Users.Add(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {Username} registered successfully", user.Username);

        return new UserRegisterResponse
        {
            Message = "User registered successfully",
            UserId = userId,
            Token = _tokenService.GenerateToken(user.Id, user.Username, user.Email)
        };
    }

    public async Task<UserLoginResponse> LoginAsync(UserLoginRequest request)
    {
        var user = _unitOfWork.Users.Get(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid email or password");

        user.LastLogin = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {Username} logged in successfully", user.Username);

        return new UserLoginResponse
        {
            Message = "Login successful",
            Token = _tokenService.GenerateToken(user.Id, user.Username, user.Email),
            UserId = user.Id
        };
    }

    public async Task<UserProfileResponse> GetProfileAsync(string userId)
    {
        var user = _unitOfWork.Users.Get(u => u.Id == userId);
        if (user == null)
            throw new NotFoundException("User not found");

        return await Task.FromResult(new UserProfileResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            RegisteredAt = user.RegisteredAt
        });
    }
}
