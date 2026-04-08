namespace quiz.app.api.DTOs.Result;

public record SubmitResultDto(
    Guid    QuizId,
    int     Score,
    int     TotalQuestions,
    TimeSpan TimeTaken
);