using quiz.app.api.Common;
using quiz.app.api.DTOs.Result;

namespace quiz.app.api.Services.Interfaces;

public interface IResultService
{
    Task<Result<ResultResponseDto>>          SubmitAsync(Guid userId, SubmitResultDto dto, CancellationToken ct = default);
    Task<Result<IEnumerable<ResultResponseDto>>> GetMyResultsAsync(Guid userId, CancellationToken ct = default);
    Task<Result<IEnumerable<LeaderboardEntryDto>>> GetLeaderboardAsync(Guid quizId, int count, CancellationToken ct = default);
}