namespace quiz.app.api.DTOs.Account;

public record LoginDto(
    string Email,
    string Password
);