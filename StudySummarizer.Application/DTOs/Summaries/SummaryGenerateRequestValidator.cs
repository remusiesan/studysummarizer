using FluentValidation;

namespace StudySummarizer.Application.DTOs.Summaries;

public class SummaryGenerateRequestValidator : AbstractValidator<SummaryGenerateRequest>
{
    private static readonly string[] AllowedSummaryTypes = ["standard", "concise", "detailed"];

    public SummaryGenerateRequestValidator()
    {
        RuleFor(x => x.SummaryType)
            .NotEmpty().WithMessage("Summary type is required")
            .Must(t => AllowedSummaryTypes.Contains(t.ToLower()))
            .WithMessage($"Summary type must be one of: {string.Join(", ", AllowedSummaryTypes)}");
    }
}
