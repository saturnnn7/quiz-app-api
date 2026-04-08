namespace quiz.app.api.Models;

public class Question
{
    public Guid   Id            { get; set; } = Guid.NewGuid();
    public Guid   QuizId        { get; set; }
    public string Text          { get; set; } = string.Empty;
    public string AnswerA       { get; set; } = string.Empty;
    public string AnswerB       { get; set; } = string.Empty;
    public string AnswerC       { get; set; } = string.Empty;
    public string AnswerD       { get; set; } = string.Empty;
    public int    CorrectAnswer { get; set; } // 0=A, 1=B, 2=C, 3=D
    public int    Order         { get; set; } = 0;

    // Navigation property
    public Quiz Quiz { get; set; } = null!;
}