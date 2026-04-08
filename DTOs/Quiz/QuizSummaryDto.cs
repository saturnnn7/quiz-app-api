namespace quiz.app.api.DTOs.Quiz;

public record QuizSummaryDto(
    Guid     Id,
    string   Title,
    string   Description,
    string   AuthorUsername,
    int      QuestionCount,
    int      PlayCount,
    DateTime CreatedAt
);