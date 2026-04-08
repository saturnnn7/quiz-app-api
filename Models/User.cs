namespace quiz.app.api.Models;

public class User
{
    public Guid     Id           { get; set; } = Guid.NewGuid();
    public string   Username     { get; set; } = string.Empty;
    public string   Email        { get; set; } = string.Empty;
    public string   PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt    { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Quiz>   Quizzes { get; set; } = [];
    public ICollection<ResultEntity> Results { get; set; } = [];
}