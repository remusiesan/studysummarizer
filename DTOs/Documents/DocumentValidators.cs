using FluentValidation;

namespace StudySummarizer.DTOs.Documents;

public class DocumentUploadRequestValidator : AbstractValidator<DocumentUploadRequest>
{
    public DocumentUploadRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Document title is required")
            .MaximumLength(255).WithMessage("Title cannot exceed 255 characters")
            .Matches("^[a-zA-Z0-9\\s\\-_.,()]+$").WithMessage("Title contains invalid characters");

        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required")
            .Must(f => f.Length > 0).WithMessage("File cannot be empty")
            .Must(f => f.Length <= 20 * 1024 * 1024).WithMessage("File size cannot exceed 20MB");
    }
}
