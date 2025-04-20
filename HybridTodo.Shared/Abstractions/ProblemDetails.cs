namespace HybridTodo.Shared.Abstractions;

public class ProblemDetails
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;

    public Error ToError()
    {
        var errorType = Type switch
        {
            "https://tools.ietf.org/html/rfc7231#section-6.5.1" => ErrorType.BadRequest,
            "https://tools.ietf.org/html/rfc7231#section-6.5.4" => ErrorType.NotFound,
            "https://tools.ietf.org/html/rfc7231#section-6.5.8" => ErrorType.Conflict,
            "https://tools.ietf.org/html/rfc7231#section-6.6.1" => ErrorType.InternalServerError, // Default fallback
            _ => ErrorType.Application // Fallback for unexpected types
        };

        return new Error(Title ?? "Unknown", Detail ?? "No details provided", errorType);
    }
}
