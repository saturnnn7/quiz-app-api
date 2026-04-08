using quiz.app.api.Common;
using quiz.app.api.DTOs.Account;
using quiz.app.api.Models;
using quiz.app.api.Repositories.Interfaces;
using quiz.app.api.Services.Interfaces;

namespace quiz.app.api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService   _tokenService;

    public AuthService(IUserRepository users, ITokenService tokenService)
    {
        _users        = users;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
    {
        if (await _users.EmailExistsAsync(dto.Email, ct))
            return Result.Failure<AuthResponseDto>(Error.Conflict("Email is already in use."));

        if (await _users.UsernameExistsAsync(dto.Username, ct))
            return Result.Failure<AuthResponseDto>(Error.Conflict("Username is already taken."));

        var user = new User
        {
            Username     = dto.Username,
            Email        = dto.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        };

        await _users.AddAsync(user, ct);
        await _users.SaveChangesAsync(ct);

        return Result.Success(BuildAuthResponse(user));
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(dto.Email, ct);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Result.Failure<AuthResponseDto>(Error.Unauthorized("Invalid email or password."));

        return Result.Success(BuildAuthResponse(user));
    }

    public async Task<Result<AccountResponseDto>> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct);

        if (user is null)
            return Result.Failure<AccountResponseDto>(Error.NotFound("User not found."));

        return Result.Success(MapToAccountResponse(user));
    }

    public async Task<Result<AccountResponseDto>> UpdateProfileAsync(Guid userId, UpdateAccountDto dto, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct);

        if (user is null)
            return Result.Failure<AccountResponseDto>(Error.NotFound("User not found."));

        if (dto.Username is not null)
        {
            if (await _users.UsernameExistsAsync(dto.Username, ct))
                return Result.Failure<AccountResponseDto>(Error.Conflict("Username is already taken."));
            user.Username = dto.Username;
        }

        if (dto.Email is not null)
        {
            if (await _users.EmailExistsAsync(dto.Email, ct))
                return Result.Failure<AccountResponseDto>(Error.Conflict("Email is already in use."));
            user.Email = dto.Email.ToLower();
        }

        if (dto.NewPassword is not null)
        {
            if (dto.CurrentPassword is null || !BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                return Result.Failure<AccountResponseDto>(Error.Unauthorized("Current password is incorrect."));

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        }

        _users.Update(user);
        await _users.SaveChangesAsync(ct);

        return Result.Success(MapToAccountResponse(user));
    }

    // --- Private helpers ---

    private AuthResponseDto BuildAuthResponse(User user) => new(
        Token:     _tokenService.GenerateToken(user),
        ExpiresAt: _tokenService.GetExpiration(),
        Account:   MapToAccountResponse(user)
    );

    private static AccountResponseDto MapToAccountResponse(User user) => new(
        user.Id, user.Username, user.Email, user.CreatedAt
    );
}