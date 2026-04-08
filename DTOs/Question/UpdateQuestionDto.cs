namespace quiz.app.api.DTOs.Question;

public record UpdateQuestionDto(
    string? Text,
    string? AnswerA,
    string? AnswerB,
    string? AnswerC,
    string? AnswerD,
    int?    CorrectAnswer,
    int?    Order
);