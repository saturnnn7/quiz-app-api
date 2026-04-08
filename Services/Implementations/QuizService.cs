using quiz.app.api.Common;
using quiz.app.api.DTOs.Question;
using quiz.app.api.DTOs.Quiz;
using quiz.app.api.Models;
using quiz.app.api.Repositories.Interfaces;
using quiz.app.api.Services.Interfaces;

namespace quiz.app.api.Services.Implementations;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizzes;

    public QuizService(IQuizRepository quizzes)
    {
        _quizzes = quizzes;
    }

    public async Task<Result<QuizDetailDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var quiz = await _quizzes.GetByIdWithQuestionsAsync(id, ct);

        if (quiz is null)
            return Result.Failure<QuizDetailDto>(Error.NotFound("Quiz not found."));

        return Result.Success(MapToDetail(quiz));
    }

    public async Task<Result<(IEnumerable<QuizSummaryDto> Items, int TotalCount)>> SearchAsync(
        QuizSearchDto dto, CancellationToken ct = default)
    {
        var (items, total) = await _quizzes.SearchAsync(dto.Query, dto.Page, dto.PageSize, ct);
        return Result.Success((items.Select(MapToSummary), total));
    }

    public async Task<Result<IEnumerable<QuizSummaryDto>>> GetTopAsync(int count, CancellationToken ct = default)
    {
        var quizzes = await _quizzes.GetTopAsync(count, ct);
        return Result.Success(quizzes.Select(MapToSummary));
    }

    public async Task<Result<IEnumerable<QuizSummaryDto>>> GetMyQuizzesAsync(Guid authorId, CancellationToken ct = default)
    {
        var quizzes = await _quizzes.GetByAuthorAsync(authorId, ct);
        return Result.Success(quizzes.Select(MapToSummary));
    }

    public async Task<Result<QuizDetailDto>> CreateAsync(Guid authorId, CreateQuizDto dto, CancellationToken ct = default)
    {
        var quiz = new Quiz
        {
            AuthorId    = authorId,
            Title       = dto.Title,
            Description = dto.Description,
            Questions   = dto.Questions.Select(q => new Question
            {
                Text          = q.Text,
                AnswerA       = q.AnswerA,
                AnswerB       = q.AnswerB,
                AnswerC       = q.AnswerC,
                AnswerD       = q.AnswerD,
                CorrectAnswer = q.CorrectAnswer,
                Order         = q.Order,
            }).ToList()
        };

        await _quizzes.AddAsync(quiz, ct);
        await _quizzes.SaveChangesAsync(ct);

        var created = await _quizzes.GetByIdWithQuestionsAsync(quiz.Id, ct);
        return Result.Success(MapToDetail(created!));
    }

    public async Task<Result<QuizDetailDto>> UpdateAsync(Guid quizId, Guid requesterId, UpdateQuizDto dto, CancellationToken ct = default)
    {
        var quiz = await _quizzes.GetByIdWithQuestionsAsync(quizId, ct);

        if (quiz is null)
            return Result.Failure<QuizDetailDto>(Error.NotFound("Quiz not found."));

        if (quiz.AuthorId != requesterId)
            return Result.Failure<QuizDetailDto>(Error.Forbidden("You are not the author of this quiz."));

        if (dto.Title is not null)       quiz.Title       = dto.Title;
        if (dto.Description is not null) quiz.Description = dto.Description;
        quiz.UpdatedAt = DateTime.UtcNow;

        _quizzes.Update(quiz);
        await _quizzes.SaveChangesAsync(ct);

        return Result.Success(MapToDetail(quiz));
    }

    public async Task<Result> DeleteAsync(Guid quizId, Guid requesterId, CancellationToken ct = default)
    {
        var quiz = await _quizzes.GetByIdAsync(quizId, ct);

        if (quiz is null)
            return Result.Failure(Error.NotFound("Quiz not found."));

        if (quiz.AuthorId != requesterId)
            return Result.Failure(Error.Forbidden("You are not the author of this quiz."));

        _quizzes.Remove(quiz);
        await _quizzes.SaveChangesAsync(ct);

        return Result.Success();
    }

    // --- Private helpers ---

    private static QuizSummaryDto MapToSummary(Quiz q) => new(
        q.Id, q.Title, q.Description,
        q.Author.Username, q.Questions.Count, q.PlayCount, q.CreatedAt
    );

    private static QuizDetailDto MapToDetail(Quiz q) => new(
        q.Id, q.Title, q.Description, q.Author.Username,
        q.PlayCount, q.CreatedAt, q.UpdatedAt,
        q.Questions.Select(MapToQuestionResponse)
    );

    private static QuestionResponseDto MapToQuestionResponse(Question q) => new(
        q.Id, q.Text, q.AnswerA, q.AnswerB, q.AnswerC, q.AnswerD, q.CorrectAnswer, q.Order
    );
}