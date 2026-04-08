namespace quiz.app.api.Common;

public class ApiResponse<T>
{
    public bool   Success  { get; init; }
    public string Message  { get; init; } = string.Empty;
    public T?     Data     { get; init; }
    public IEnumerable<string> Errors { get; init; } = [];

    public static ApiResponse<T> Ok(T data, string message = "Success") =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors ?? [] };
}

public class ApiResponse
{
    public bool   Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public IEnumerable<string> Errors { get; init; } = [];

    public static ApiResponse Ok(string message = "Success") =>
        new() { Success = true, Message = message };

    public static ApiResponse Fail(string message, IEnumerable<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors ?? [] };
}

public class PagedResponse<T>
{
    public bool              Success    { get; init; }
    public string            Message    { get; init; } = string.Empty;
    public IEnumerable<T>    Data       { get; init; } = [];
    public PaginationMeta    Pagination { get; init; } = new();

    public static PagedResponse<T> Ok(IEnumerable<T> data, PaginationMeta pagination) =>
        new() { Success = true, Message = "Success", Data = data, Pagination = pagination };
}

public class PaginationMeta
{
    public int Page       { get; init; }
    public int PageSize   { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNext   => Page < TotalPages;
    public bool HasPrev   => Page > 1;
}