namespace StudySummarizer;

public static class Constants
{
    public static class Jwt
    {
        public const string SchemeId = "Bearer";
        public const string SchemeName = "Authorization";
        public const string Scheme = "bearer";
        public const string BearerFormat = "JWT";
        public const string Description = "JWT Token Authorization";
        public const string DefaultSecret = "your-secret-key-change-this-in-production";
        public const string DefaultIssuer = "StudySummarizer";
        public const string DefaultAudience = "StudySummarizerAPI";
        public const string ConfigKeySecret = "Jwt:Secret";
        public const string ConfigKeyIssuer = "Jwt:Issuer";
        public const string ConfigKeyAudience = "Jwt:Audience";
    }

    public static class Database
    {
        public const string SqliteConnection = "Data Source=studysummarizer.db";
    }

    public static class Cors
    {
        public const string AllowAllPolicy = "AllowAll";
    }

    public static class Swagger
    {
        public const string JsonEndpoint = "/swagger/v1/swagger.json";
        public const string Title = "Document Management API v1";
        public const string RoutePrefix = "api/v1";
    }

    public static class Logging
    {
        public const string LogFilePattern = "logs/app-.txt";
        public const string UnhandledExceptionMessage = "An unhandled exception occurred";
    }

    public static class ContentTypes
    {
        public const string Json = "application/json";
    }

    public static class ErrorCodes
    {
        public const string InternalError = "INTERNAL_ERROR";
        public const string NotFound = "NOT_FOUND";
        public const string Unauthorized = "UNAUTHORIZED";
        public const string ValidationError = "VALIDATION_ERROR";
    }

    public static class DocumentStatus
    {
        public const string Uploaded = "Uploaded";
        public const string Processing = "Processing";
        public const string Summarized = "Summarized";
    }

    public static class SummaryStatus
    {
        public const string Generated = "Generated";
        public const string Processing = "Processing";
        public const string Failed = "Failed";
    }

    public static class SummaryTone
    {
        public const string Neutral = "neutral";
        public const string Formal = "formal";
        public const string Casual = "casual";
        public const string Academic = "academic";
    }

    public static class ApiRoutes
    {
        public const string Users = "api/v1/users";
        public const string Documents = "api/v1/documents";
        public const string Models = "api/ai/models";
    }

    public static class ApiEndpoints
    {
        public const string Register = "register";
        public const string Login = "login";
        public const string Profile = "profile";
        public const string Summarize = "summarize";
        public const string Summary = "summary";
    }

    public static class ResponseMessages
    {
        public const string ModelUpdatedSuccessfully = "Model updated successfully";
        public const string SummaryUpdatedSuccessfully = "Summary updated successfully";
        public const string ProcessingStatus = "Processing";
    }

    public static class ExceptionMessages
    {
        public const string NotFoundMessage = "not found";
        public const string ModelNotFound = "AI Model not found";
        public const string DocumentNotFound = "Document not found";
        public const string SummaryNotFound = "Summary not found";
        public const string UserNotFound = "User not found";
    }

    public static class AllowedFileExtensions
    {
        public const string Pdf = ".pdf";
        public const string Docx = ".docx";
        public const string Txt = ".txt";
    }
}
