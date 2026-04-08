using quiz.app.api.Models;

namespace quiz.app.api.Services.Interfaces;

public interface ITokenService
{
    string   GenerateToken(User user);
    DateTime GetExpiration();
}