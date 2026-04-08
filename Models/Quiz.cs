namespace quiz.app.api.Models;

public class Quiz
{
    public Guid     Id          { get; set; } = Guid.NewGuid();
    public string   Title       { get; set; } = string.Empty;
    public string   Description { get; set; } = string.Empty;
    public Guid     AuthorId    { get; set; }
    public int      PlayCount   { get; set; } = 0;
    public DateTime CreatedAt   { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt   { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User                 Author    { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<ResultEntity> Results { get; set; } = [];
}