namespace quiz.app.api.DTOs.Quiz;

public record QuizSearchDto(
    string? Query,
    int     Page     = 1,
    int     PageSize = 10
);