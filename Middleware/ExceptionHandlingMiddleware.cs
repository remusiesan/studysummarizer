using StudySummarizer.DTOs;
using StudySummarizer.Exceptions;
using System.Net;

namespace StudySummarizer.Middleware;

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
            _logger.LogError(ex, Constants.Logging.UnhandledExceptionMessage);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = Constants.ContentTypes.Json;

        var response = exception switch
        {
            ValidationException vEx => new ApiResponse
            {
                Success = false,
                Message = vEx.Message,
                Errors = [vEx.Message],
                Timestamp = DateTime.UtcNow
            },
            NotFoundException nfEx => new ApiResponse
            {
                Success = false,
                Message = nfEx.Message,
                Errors = [nfEx.Message],
                Timestamp = DateTime.UtcNow
            },
            UnauthorizedException uEx => new ApiResponse
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
            ValidationException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
