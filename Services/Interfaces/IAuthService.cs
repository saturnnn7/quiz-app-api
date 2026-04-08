using quiz.app.api.Common;
using quiz.app.api.DTOs.Account;

namespace quiz.app.api.Services.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponseDto>>    RegisterAsync(RegisterDto dto, CancellationToken ct = default);
    Task<Result<AuthResponseDto>>    LoginAsync(LoginDto dto, CancellationToken ct = default);
    Task<Result<AccountResponseDto>> GetProfileAsync(Guid userId, CancellationToken ct = default);
    Task<Result<AccountResponseDto>> UpdateProfileAsync(Guid userId, UpdateAccountDto dto, CancellationToken ct = default);
}