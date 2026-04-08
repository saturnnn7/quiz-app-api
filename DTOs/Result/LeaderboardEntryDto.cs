namespace quiz.app.api.DTOs.Result;

public record LeaderboardEntryDto(
    string   Username,
    int      BestScore,
    int      TotalQuestions,
    int      Percentage,
    DateTime CompletedAt
);