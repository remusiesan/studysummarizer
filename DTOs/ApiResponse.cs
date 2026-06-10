namespace StudySummarizer.DTOs;

/// <summary>
/// Standard API response wrapper for all endpoints
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = [];
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Timestamp = DateTime.UtcNow
        };
    }

    public static ApiResponse<T> FailureResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? [],
            Timestamp = DateTime.UtcNow
        };
    }
}

/// <summary>
/// Standard API response for non-generic operations
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = [];
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse SuccessResponse(string message = "Operation successful")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message,
            Timestamp = DateTime.UtcNow
        };
    }

    public static ApiResponse FailureResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? [],
            Timestamp = DateTime.UtcNow
        };
    }
}
