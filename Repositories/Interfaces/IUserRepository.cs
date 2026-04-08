using quiz.app.api.Models;

namespace quiz.app.api.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<bool>  EmailExistsAsync(string email, CancellationToken ct = default);
    Task<bool>  UsernameExistsAsync(string username, CancellationToken ct = default);
}