namespace HybridTodo.Shared.Abstractions;

public record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.Failure);
    public static readonly Error InvalidOperation = new("Error.InvalidOperation", "The requested operation is invalid", ErrorType.Application);

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);

    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    public static Error Problem(string code, string message) => new(code, message, ErrorType.Problem);

    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    public static Error BadRequest(string code, string message) => new(code, message, ErrorType.BadRequest);

    public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);

    public static Error Application(string code, string message) => new(code, message, ErrorType.Application);

    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

    public static Error InternalServerError(string code, string message) => new(code, message, ErrorType.InternalServerError);
}
