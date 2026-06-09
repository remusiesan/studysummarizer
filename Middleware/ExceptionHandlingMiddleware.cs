using StudySummarizer.Exceptions;
using System.Net;
using System.Text.Json;

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

        var response = new
        {
            error = exception.Message,
            code = Constants.ErrorCodes.InternalError,
            timestamp = DateTime.UtcNow
        };

        if (exception is ApiException apiEx)
        {
            context.Response.StatusCode = apiEx.StatusCode;
            response = new
            {
                error = apiEx.Message,
                code = apiEx.ErrorCode,
                timestamp = DateTime.UtcNow
            };
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}
