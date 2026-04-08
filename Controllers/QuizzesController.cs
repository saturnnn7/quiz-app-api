using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quiz.app.api.Common;
using quiz.app.api.DTOs.Quiz;
using quiz.app.api.Services.Interfaces;

namespace quiz.app.api.Controllers;

public class QuizzesController : BaseController
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] QuizSearchDto dto, CancellationToken ct)
    {
        var result = await _quizService.SearchAsync(dto, ct);

        if (result.IsFailure)
            return HandleError<object>(result.Error);

        var (items, total) = result.Value;
        var pagination = new PaginationMeta
        {
            Page       = dto.Page,
            PageSize   = dto.PageSize,
            TotalCount = total,
        };

        return Ok(PagedResponse<QuizSummaryDto>.Ok(items, pagination));
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTop([FromQuery] int count = 10, CancellationToken ct = default)
    {
        var result = await _quizService.GetTopAsync(count, ct);
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyQuizzes(CancellationToken ct)
    {
        var authorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result   = await _quizService.GetMyQuizzesAsync(authorId, ct);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _quizService.GetByIdAsync(id, ct);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuizDto dto, CancellationToken ct)
    {
        var authorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result   = await _quizService.CreateAsync(authorId, dto, ct);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQuizDto dto, CancellationToken ct)
    {
        var requesterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result      = await _quizService.UpdateAsync(id, requesterId, dto, ct);
        return HandleResult(result);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var requesterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result      = await _quizService.DeleteAsync(id, requesterId, ct);
        return HandleResult(result);
    }
}