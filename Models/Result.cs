namespace quiz.app.api.Models;

public class ResultEntity
{
    public Guid     Id          { get; set; } = Guid.NewGuid();
    public Guid     UserId      { get; set; }
    public Guid     QuizId      { get; set; }
    public int      Score       { get; set; }
    public int      TotalQuestions { get; set; }
    public TimeSpan TimeTaken   { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = null!;
    public Quiz Quiz { get; set; } = null!;
}