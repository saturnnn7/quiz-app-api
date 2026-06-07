namespace quiz.app.api.DTOs.Question;

public record CreateQuestionDto(
    string Text,
    string AnswerA,
    string AnswerB,
    string? AnswerC,
    string? AnswerD,
    int CorrectAnswer,
    int AnswerCount = 4,
    int Order = 0
);
