namespace quiz.app.api.DTOs.Account;

public record AccountResponseDto(
    Guid     Id,
    string   Username,
    string   Email,
    DateTime CreatedAt
);