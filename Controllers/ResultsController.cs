using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quiz.app.api.DTOs.Result;
using quiz.app.api.Services.Interfaces;

namespace quiz.app.api.Controllers;

public class ResultsController : BaseController
{
    private readonly IResultService _resultService;

    public ResultsController(IResultService resultService)
    {
        _resultService = resultService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitResultDto dto, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _resultService.SubmitAsync(userId, dto, ct);
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyResults(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _resultService.GetMyResultsAsync(userId, ct);
        return HandleResult(result);
    }

    [HttpGet("leaderboard/{quizId:guid}")]
    public async Task<IActionResult> GetLeaderboard(Guid quizId, [FromQuery] int count = 10, CancellationToken ct = default)
    {
        var result = await _resultService.GetLeaderboardAsync(quizId, count, ct);
        return HandleResult(result);
    }
}