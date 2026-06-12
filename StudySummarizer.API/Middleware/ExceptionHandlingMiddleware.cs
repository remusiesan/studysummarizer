using StudySummarizer.Application.DTOs;
using System.Net;

using DomainExceptions = StudySummarizer.Domain.Exceptions;

namespace StudySummarizer.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            FluentValidation.ValidationException fvEx => new ApiResponse
            {
                Success = false,
                Message = "Validation failed",
                Errors = fvEx.Errors.Select(e => e.ErrorMessage).ToList(),
                Timestamp = DateTime.UtcNow
            },
            DomainExceptions.ValidationException vEx => new ApiResponse
            {
                Success = false,
                Message = vEx.Message,
                Errors = [vEx.Message],
                Timestamp = DateTime.UtcNow
            },
            DomainExceptions.NotFoundException nfEx => new ApiResponse
            {
                Success = false,
                Message = nfEx.Message,
                Errors = [nfEx.Message],
                Timestamp = DateTime.UtcNow
            },
            DomainExceptions.UnauthorizedException uEx => new ApiResponse
            {
                Success = false,
                Message = uEx.Message,
                Errors = [uEx.Message],
                Timestamp = DateTime.UtcNow
            },
            _ => new ApiResponse
            {
                Success = false,
                Message = "An unexpected error occurred",
                Errors = [exception.Message],
                Timestamp = DateTime.UtcNow
            }
        };

        context.Response.StatusCode = exception switch
        {
            FluentValidation.ValidationException => (int)HttpStatusCode.BadRequest,
            DomainExceptions.ValidationException => (int)HttpStatusCode.BadRequest,
            DomainExceptions.NotFoundException => (int)HttpStatusCode.NotFound,
            DomainExceptions.UnauthorizedException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
