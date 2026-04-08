using quiz.app.api.Common;
using quiz.app.api.DTOs.Result;
using quiz.app.api.Models;
using quiz.app.api.Repositories.Interfaces;
using quiz.app.api.Services.Interfaces;

namespace quiz.app.api.Services.Implementations;

public class ResultService : IResultService
{
    private readonly IResultRepository _results;
    private readonly IQuizRepository   _quizzes;

    public ResultService(IResultRepository results, IQuizRepository quizzes)
    {
        _results = results;
        _quizzes = quizzes;
    }

    public async Task<Result<ResultResponseDto>> SubmitAsync(Guid userId, SubmitResultDto dto, CancellationToken ct = default)
    {
        var quiz = await _quizzes.GetByIdAsync(dto.QuizId, ct);

        if (quiz is null)
            return Result.Failure<ResultResponseDto>(Error.NotFound("Quiz not found."));

        var entity = new ResultEntity
        {
            UserId         = userId,
            QuizId         = dto.QuizId,
            Score          = dto.Score,
            TotalQuestions = dto.TotalQuestions,
            TimeTaken      = dto.TimeTaken,
        };

        await _results.AddAsync(entity, ct);
        await _quizzes.IncrementPlayCountAsync(dto.QuizId, ct);
        await _results.SaveChangesAsync(ct);

        return Result.Success(new ResultResponseDto(
            entity.Id,
            entity.QuizId,
            quiz.Title,
            string.Empty,
            entity.Score,
            entity.TotalQuestions,
            (int)Math.Round((double)entity.Score / entity.TotalQuestions * 100),
            entity.TimeTaken,
            entity.CompletedAt
        ));
    }

    public async Task<Result<IEnumerable<ResultResponseDto>>> GetMyResultsAsync(Guid userId, CancellationToken ct = default)
    {
        var results = await _results.GetByUserAsync(userId, ct);
        return Result.Success(results.Select(MapToResponse));
    }

    public async Task<Result<IEnumerable<LeaderboardEntryDto>>> GetLeaderboardAsync(Guid quizId, int count, CancellationToken ct = default)
    {
        var quiz = await _quizzes.GetByIdAsync(quizId, ct);

        if (quiz is null)
            return Result.Failure<IEnumerable<LeaderboardEntryDto>>(Error.NotFound("Quiz not found."));

        var results = await _results.GetLeaderboardAsync(quizId, count, ct);

        return Result.Success(results.Select(r => new LeaderboardEntryDto(
            r.User.Username,
            r.Score,
            r.TotalQuestions,
            (int)Math.Round((double)r.Score / r.TotalQuestions * 100),
            r.CompletedAt
        )));
    }

    private static ResultResponseDto MapToResponse(ResultEntity r) => new(
        r.Id, r.QuizId, r.Quiz.Title, r.User.Username,
        r.Score, r.TotalQuestions,
        (int)Math.Round((double)r.Score / r.TotalQuestions * 100),
        r.TimeTaken, r.CompletedAt
    );
}