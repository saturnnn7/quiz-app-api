using quiz.app.api.Models;

namespace quiz.app.api.Repositories.Interfaces;

public interface IResultRepository : IRepository<ResultEntity>
{
    Task<IEnumerable<ResultEntity>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task<IEnumerable<ResultEntity>> GetByQuizAsync(Guid quizId, CancellationToken ct = default);
    Task<IEnumerable<ResultEntity>> GetLeaderboardAsync(Guid quizId, int count, CancellationToken ct = default);
}