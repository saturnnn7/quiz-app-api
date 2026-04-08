namespace quiz.app.api.DTOs.Account;

// Internal DTO used by the service layer after validation
public record CreateAccountDto(
    string Username,
    string Email,
    string PasswordHash
);