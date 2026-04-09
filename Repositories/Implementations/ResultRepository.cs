using Microsoft.EntityFrameworkCore;
using quiz.app.api.Data;
using quiz.app.api.Models;
using quiz.app.api.Repositories.Interfaces;

namespace quiz.app.api.Repositories.Implementations;

public class ResultRepository : BaseRepository<ResultEntity>, IResultRepository
{
    public ResultRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<ResultEntity>> GetByUserAsync(Guid userId, CancellationToken ct = default) =>
        await DbSet
            .Include(r => r.Quiz)
            .Include(r => r.User)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CompletedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<ResultEntity>> GetByQuizAsync(Guid quizId, CancellationToken ct = default) =>
        await DbSet
            .Include(r => r.User)
            .Where(r => r.QuizId == quizId)
            .OrderByDescending(r => r.CompletedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<ResultEntity>> GetLeaderboardAsync(Guid quizId, int count, CancellationToken ct = default) =>
        await DbSet
            .Include(r => r.User)
            .Where(r => r.QuizId == quizId)
            .OrderByDescending(r => r.Score)
            .ThenBy(r => r.TimeTaken)
            .Take(count)
            .ToListAsync(ct);
}