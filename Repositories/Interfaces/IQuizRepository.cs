using quiz.app.api.Models;

namespace quiz.app.api.Repositories.Interfaces;

public interface IQuizRepository : IRepository<Quiz>
{
    Task<Quiz?>          GetByIdWithQuestionsAsync(Guid id, CancellationToken ct = default);
    Task<(IEnumerable<Quiz> Items, int TotalCount)> SearchAsync(string? query, int page, int pageSize, CancellationToken ct = default);
    Task<IEnumerable<Quiz>> GetTopAsync(int count, CancellationToken ct = default);
    Task<IEnumerable<Quiz>> GetByAuthorAsync(Guid authorId, CancellationToken ct = default);
    Task                 IncrementPlayCountAsync(Guid quizId, CancellationToken ct = default);
}