namespace quiz.app.api.DTOs.Question;

public record QuestionResponseDto(
    Guid   Id,
    string Text,
    string AnswerA,
    string AnswerB,
    string AnswerC,
    string AnswerD,
    int    CorrectAnswer,
    int    Order
);