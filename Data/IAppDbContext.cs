using Microsoft.EntityFrameworkCore;
using quiz.app.api.Models;

namespace quiz.app.api.Data;

public interface IAppDbContext
{
    DbSet<User>         Users     { get; }
    DbSet<Quiz>         Quizzes   { get; }
    DbSet<Question>     Questions { get; }
    DbSet<ResultEntity> Results   { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}