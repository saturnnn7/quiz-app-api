using FluentValidation;
using quiz.app.api.DTOs.Quiz;

namespace quiz.app.api.Validators;

public class CreateQuizDtoValidator : AbstractValidator<CreateQuizDto>
{
    public CreateQuizDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.Questions)
            .NotEmpty().WithMessage("Quiz must have at least one question.")
            .Must(q => q.Count() <= 50).WithMessage("Quiz cannot have more than 50 questions.");

        RuleForEach(x => x.Questions).SetValidator(new CreateQuestionDtoValidator());
    }
}