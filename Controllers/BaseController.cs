using Microsoft.AspNetCore.Mvc;
using quiz.app.api.Common;

namespace quiz.app.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    // Maps a Result<T> to the correct HTTP response
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(ApiResponse<T>.Ok(result.Value));

        return HandleError<T>(result.Error);
    }

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return Ok(ApiResponse.Ok());

        return HandleError(result.Error);
    }

    protected IActionResult HandleError<T>(Error error) => error.Type switch
    {
        ErrorType.NotFound     => NotFound(ApiResponse<T>.Fail(error.Message)),
        ErrorType.Validation   => BadRequest(ApiResponse<T>.Fail(error.Message)),
        ErrorType.Conflict     => Conflict(ApiResponse<T>.Fail(error.Message)),
        ErrorType.Unauthorized => Unauthorized(ApiResponse<T>.Fail(error.Message)),
        ErrorType.Forbidden    => StatusCode(403, ApiResponse<T>.Fail(error.Message)),
        _                      => StatusCode(500, ApiResponse<T>.Fail(error.Message)),
    };

    protected IActionResult HandleError(Error error) => error.Type switch
    {
        ErrorType.NotFound     => NotFound(ApiResponse.Fail(error.Message)),
        ErrorType.Validation   => BadRequest(ApiResponse.Fail(error.Message)),
        ErrorType.Conflict     => Conflict(ApiResponse.Fail(error.Message)),
        ErrorType.Unauthorized => Unauthorized(ApiResponse.Fail(error.Message)),
        ErrorType.Forbidden    => StatusCode(403, ApiResponse.Fail(error.Message)),
        _                      => StatusCode(500, ApiResponse.Fail(error.Message)),
    };
}