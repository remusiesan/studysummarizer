namespace StudySummarizer.Domain.Exceptions;

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message)
        : base(message, 401, "UNAUTHORIZED") { }
}
