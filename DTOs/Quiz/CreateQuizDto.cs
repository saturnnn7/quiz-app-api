using quiz.app.api.DTOs.Question;

namespace quiz.app.api.DTOs.Quiz;

public record CreateQuizDto(
    string                      Title,
    string                      Description,
    IEnumerable<CreateQuestionDto> Questions
);