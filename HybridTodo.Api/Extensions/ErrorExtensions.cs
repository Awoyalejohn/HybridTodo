using HybridTodo.Shared.Abstractions;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace HybridTodo.Api.Extensions;

internal static class ErrorExtensions
{
    public static ProblemDetails ToProblemDetails(this Error error)
    {
        var problemDetails = new ProblemDetails()
        {
            Title = error.Code,
            Detail = error.Message,
            Type = error.Type switch
            {
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
                ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                ErrorType.Application => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                ErrorType.InternalServerError => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"

            },
            Status = error.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.BadRequest => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Application => StatusCodes.Status500InternalServerError,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.InternalServerError => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            },
        };

        return problemDetails;
    }
}
