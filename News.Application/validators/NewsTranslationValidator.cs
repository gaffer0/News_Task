using FluentValidation;
using News.Domain.Entities;

public class NewsTranslationValidator : AbstractValidator<NewTranslation>
{
    public NewsTranslationValidator()
    {
        RuleFor(t => t.NewId)
            .GreaterThan(0).WithMessage("NewId must be a positive integer.");

        RuleFor(t => t.Language1)
            .NotEmpty().WithMessage("Language1 is required.")
            .Length(2, 5).WithMessage("Language1 must be between 2 and 5 characters.");

        RuleFor(t => t.Language1Title)
            .NotEmpty().WithMessage("Language1Title is required.")
            .MaximumLength(100).WithMessage("Language1Title cannot exceed 100 characters.");

        RuleFor(t => t.Language1Content)
            .NotEmpty().WithMessage("Language1Content is required.")
            .MinimumLength(10).WithMessage("Language1Content must be at least 10 characters long.");

        RuleFor(t => t.Language2)
            .NotEmpty().WithMessage("Language2 is required.")
            .Length(2, 5).WithMessage("Language2 must be between 2 and 5 characters.");

        RuleFor(t => t.Language2Title)
            .NotEmpty().WithMessage("Language2Title is required.")
            .MaximumLength(100).WithMessage("Language2Title cannot exceed 100 characters.");

        RuleFor(t => t.Language2Content)
            .NotEmpty().WithMessage("Language2Content is required.")
            .MinimumLength(10).WithMessage("Language2Content must be at least 10 characters long.");
    }
}
