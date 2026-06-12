using StudySummarizer.Application.Services.Interfaces;

namespace StudySummarizer.API.Adapters;

public class FormFileAdapter : IFileUpload
{
    private readonly IFormFile _formFile;

    public FormFileAdapter(IFormFile formFile)
    {
        _formFile = formFile;
    }

    public string? FileName => _formFile.FileName;
    public string? ContentType => _formFile.ContentType;
    public long Length => _formFile.Length;
    public Task CopyToAsync(Stream target) => _formFile.CopyToAsync(target);
}
