using FluentValidation;
using quiz.app.api.DTOs.Result;

namespace quiz.app.api.Validators;

public class SubmitResultDtoValidator : AbstractValidator<SubmitResultDto>
{
    public SubmitResultDtoValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("QuizId is required.");

        RuleFor(x => x.Score)
            .GreaterThanOrEqualTo(0).WithMessage("Score cannot be negative.");

        RuleFor(x => x.TotalQuestions)
            .GreaterThan(0).WithMessage("TotalQuestions must be greater than zero.");

        RuleFor(x => x.Score)
            .LessThanOrEqualTo(x => x.TotalQuestions)
            .WithMessage("Score cannot exceed TotalQuestions.");
    }
}