namespace StudySummarizer.Domain.Exceptions;

public class ValidationException : ApiException
{
    public ValidationException(string message)
        : base(message, 400, "VALIDATION_ERROR") { }
}
