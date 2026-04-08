using quiz.app.api.Common;
using quiz.app.api.DTOs.Quiz;

namespace quiz.app.api.Services.Interfaces;

public interface IQuizService
{
    Task<Result<QuizDetailDto>>              GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<(IEnumerable<QuizSummaryDto> Items, int TotalCount)>> SearchAsync(QuizSearchDto dto, CancellationToken ct = default);
    Task<Result<IEnumerable<QuizSummaryDto>>> GetTopAsync(int count, CancellationToken ct = default);
    Task<Result<IEnumerable<QuizSummaryDto>>> GetMyQuizzesAsync(Guid authorId, CancellationToken ct = default);
    Task<Result<QuizDetailDto>>              CreateAsync(Guid authorId, CreateQuizDto dto, CancellationToken ct = default);
    Task<Result<QuizDetailDto>>              UpdateAsync(Guid quizId, Guid requesterId, UpdateQuizDto dto, CancellationToken ct = default);
    Task<Result>                             DeleteAsync(Guid quizId, Guid requesterId, CancellationToken ct = default);
}