using FluentValidation;
using quiz.app.api.DTOs.Question;

namespace quiz.app.api.Validators;

public class CreateQuestionDtoValidator : AbstractValidator<CreateQuestionDto>
{
    public CreateQuestionDtoValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(500).WithMessage("Question text must not exceed 500 characters.");

        RuleFor(x => x.AnswerA).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AnswerB).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AnswerC).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AnswerD).NotEmpty().MaximumLength(200);

        RuleFor(x => x.CorrectAnswer)
            .InclusiveBetween(0, 3).WithMessage("CorrectAnswer must be between 0 and 3.");
    }
}