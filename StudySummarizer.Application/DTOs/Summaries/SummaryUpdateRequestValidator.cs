using FluentValidation;

namespace StudySummarizer.Application.DTOs.Summaries;

public class SummaryUpdateRequestValidator : AbstractValidator<SummaryUpdateRequest>
{
    private static readonly string[] AllowedSummaryTypes = ["standard", "concise", "detailed"];

    public SummaryUpdateRequestValidator()
    {
        RuleFor(x => x.SummaryType)
            .NotEmpty().WithMessage("Summary type is required")
            .Must(t => AllowedSummaryTypes.Contains(t.ToLower()))
            .WithMessage($"Summary type must be one of: {string.Join(", ", AllowedSummaryTypes)}");
    }
}
