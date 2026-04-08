namespace quiz.app.api.DTOs.Account;

public record RegisterDto(
    string Username,
    string Email,
    string Password,
    string ConfirmPassword
);