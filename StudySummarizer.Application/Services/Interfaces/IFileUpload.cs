namespace StudySummarizer.Application.Services.Interfaces;

public interface IFileUpload
{
    string? FileName { get; }
    string? ContentType { get; }
    long Length { get; }
    Task CopyToAsync(Stream target);
}
