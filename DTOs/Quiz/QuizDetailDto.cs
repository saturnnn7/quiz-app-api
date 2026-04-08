using quiz.app.api.DTOs.Question;

namespace quiz.app.api.DTOs.Quiz;

public record QuizDetailDto(
    Guid                           Id,
    string                         Title,
    string                         Description,
    string                         AuthorUsername,
    int                            PlayCount,
    DateTime                       CreatedAt,
    DateTime                       UpdatedAt,
    IEnumerable<QuestionResponseDto> Questions
);