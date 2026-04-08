namespace quiz.app.api.DTOs.Quiz;

public record UpdateQuizDto(
    string? Title,
    string? Description
);