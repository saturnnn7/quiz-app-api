using Microsoft.EntityFrameworkCore;
using quiz.app.api.Data;
using quiz.app.api.Models;
using quiz.app.api.Repositories.Interfaces;

namespace quiz.app.api.Repositories.Implementations;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(u => u.Email == email.ToLower(), ct);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(u => u.Username == username, ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default) =>
        await DbSet.AnyAsync(u => u.Email == email.ToLower(), ct);

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken ct = default) =>
        await DbSet.AnyAsync(u => u.Username == username, ct);
}