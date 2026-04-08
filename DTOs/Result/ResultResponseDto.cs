namespace quiz.app.api.DTOs.Result;

public record ResultResponseDto(
    Guid     Id,
    Guid     QuizId,
    string   QuizTitle,
    string   Username,
    int      Score,
    int      TotalQuestions,
    int      Percentage,
    TimeSpan TimeTaken,
    DateTime CompletedAt
);