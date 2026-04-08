namespace quiz.app.api.DTOs.Account;

public record UpdateAccountDto(
    string? Username,
    string? Email,
    string? CurrentPassword,
    string? NewPassword
);