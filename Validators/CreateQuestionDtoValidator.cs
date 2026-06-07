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

        RuleFor(x => x.AnswerCount)
            .InclusiveBetween(2, 4).WithMessage("AnswerCount must be 2, 3, or 4.");

        RuleFor(x => x.AnswerA).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AnswerB).NotEmpty().MaximumLength(200);

        RuleFor(x => x.AnswerC)
            .NotEmpty().WithMessage("Answer C is required when answerCount >= 3.")
            .MaximumLength(200)
            .When(x => x.AnswerCount >= 3);

        RuleFor(x => x.AnswerD)
            .NotEmpty().WithMessage("Answer D is required when answerCount = 4.")
            .MaximumLength(200)
            .When(x => x.AnswerCount >= 4);

        RuleFor(x => x.CorrectAnswer)
            .Must((dto, correct) => correct >= 0 && correct < dto.AnswerCount)
            .WithMessage("CorrectAnswer must be a valid index within the number of answers.");
    }
}
