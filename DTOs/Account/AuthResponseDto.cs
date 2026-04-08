namespace quiz.app.api.DTOs.Account;

public record AuthResponseDto(
    string           Token,
    DateTime         ExpiresAt,
    AccountResponseDto Account
);