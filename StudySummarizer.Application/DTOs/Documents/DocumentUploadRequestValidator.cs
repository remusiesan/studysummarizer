using FluentValidation;

namespace StudySummarizer.Application.DTOs.Documents;

public class DocumentUploadRequestValidator : AbstractValidator<DocumentUploadRequest>
{
    private static readonly string[] AllowedExtensions =
        ["pdf", "doc", "docx", "txt", "xls", "xlsx", "ppt", "pptx", "png", "jpg", "jpeg", "gif"];

    public DocumentUploadRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Document title is required")
            .MaximumLength(255).WithMessage("Title cannot exceed 255 characters")
            .Matches("^[a-zA-Z0-9\\s\\-_.,()]+$").WithMessage("Title contains invalid characters");

        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required")
            .Must(f => f.Length > 0).WithMessage("File cannot be empty")
            .Must(f => f.Length <= 20 * 1024 * 1024).WithMessage("File size cannot exceed 20MB")
            .Must(f => HasValidExtension(f.FileName))
            .WithMessage($"File format is not allowed. Supported formats: {string.Join(", ", AllowedExtensions.Select(e => e.ToUpper()))}");
    }

    private static bool HasValidExtension(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return false;
        var ext = Path.GetExtension(fileName).TrimStart('.').ToLower();
        return AllowedExtensions.Contains(ext);
    }
}
