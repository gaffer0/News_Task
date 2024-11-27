using FluentValidation;
using News.Domain.Entities;

public class NewValidator : AbstractValidator<New>
{
    public NewValidator()
    {
        // Validate Title
        RuleFor(n => n.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(20).WithMessage("Title cannot exceed 20 characters.");

        // Validate Content
        RuleFor(n => n.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MinimumLength(10).WithMessage("Content must be at least 10 characters long.");

        // Validate CreatedDate
        RuleFor(n => n.CreatedDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("CreatedDate cannot be in the future.");

        // Validate IsFeatured
        RuleFor(n => n.IsFeatured)
            .NotNull().WithMessage("IsFeatured is required.");

        // Validate Translations collection
        RuleFor(n => n.Translations)
            .NotNull().WithMessage("Translations collection cannot be null.")
            .Must(t => t.Count > 0).WithMessage("At least one translation is required.");

        RuleForEach(n => n.Translations)
            .SetValidator(new NewsTranslationValidator());

        // Validate Images collection
        RuleFor(n => n.Image)
            .NotNull().WithMessage("Images collection cannot be null.")
            .Must(i => i.Count > 0).WithMessage("At least one image is required.");
    }
}
