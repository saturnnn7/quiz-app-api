using Microsoft.EntityFrameworkCore;
using quiz.app.api.Data;
using quiz.app.api.Models;
using quiz.app.api.Repositories.Interfaces;

namespace quiz.app.api.Repositories.Implementations;

public class QuizRepository : BaseRepository<Quiz>, IQuizRepository
{
    public QuizRepository(AppDbContext context) : base(context) { }

    public async Task<Quiz?> GetByIdWithQuestionsAsync(Guid id, CancellationToken ct = default) =>
        await DbSet
            .Include(q => q.Author)
            .Include(q => q.Questions.OrderBy(q => q.Order))
            .FirstOrDefaultAsync(q => q.Id == id, ct);

    public async Task<(IEnumerable<Quiz> Items, int TotalCount)> SearchAsync(
        string? query, int page, int pageSize, CancellationToken ct = default)
    {
        var queryable = DbSet
            .Include(q => q.Author)
            .Include(q => q.Questions)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            queryable = queryable.Where(q =>
                q.Title.Contains(query) ||
                q.Description.Contains(query));

        var total = await queryable.CountAsync(ct);
        var items = await queryable
            .OrderByDescending(q => q.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<IEnumerable<Quiz>> GetTopAsync(int count, CancellationToken ct = default) =>
        await DbSet
            .Include(q => q.Author)
            .Include(q => q.Questions)
            .OrderByDescending(q => q.PlayCount)
            .Take(count)
            .ToListAsync(ct);

    public async Task<IEnumerable<Quiz>> GetByAuthorAsync(Guid authorId, CancellationToken ct = default) =>
        await DbSet
            .Include(q => q.Author)
            .Include(q => q.Questions)
            .Where(q => q.AuthorId == authorId)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(ct);

    public async Task IncrementPlayCountAsync(Guid quizId, CancellationToken ct = default)
    {
        await DbSet
            .Where(q => q.Id == quizId)
            .ExecuteUpdateAsync(s => s.SetProperty(q => q.PlayCount, q => q.PlayCount + 1), ct);
    }
}