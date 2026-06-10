namespace StudySummarizer.Exceptions;

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

public class NotFoundException : ApiException
{
    public NotFoundException(string message)
        : base(message, 404, Constants.ErrorCodes.NotFound) { }
}

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message)
        : base(message, 401, Constants.ErrorCodes.Unauthorized) { }
}

public class ValidationException : ApiException
{
    public ValidationException(string message)
        : base(message, 400, Constants.ErrorCodes.ValidationError) { }
}
