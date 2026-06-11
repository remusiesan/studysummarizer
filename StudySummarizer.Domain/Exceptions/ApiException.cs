namespace StudySummarizer.Domain.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; }
    public string ErrorCode { get; }

    public ApiException(string message, int statusCode = 400, string errorCode = "BAD_REQUEST")
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}
