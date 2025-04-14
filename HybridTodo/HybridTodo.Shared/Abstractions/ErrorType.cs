namespace HybridTodo.Shared.Abstractions;

/// <summary>
/// Represents various error types in the application.
/// </summary>
public enum ErrorType
{
    /// <summary> No error occurred. </summary>
    None,

    /// <summary> The requested resource was not found. </summary>
    NotFound,

    /// <summary> The request was malformed or invalid. </summary>
    BadRequest,

    /// <summary> Authentication is required but has failed or not been provided. </summary>
    Unauthorized,

    /// <summary> The client does not have permission to access the requested resource. </summary>
    Forbidden,

    /// <summary> A general application error occurred. </summary>
    Application,

    /// <summary> An unexpected failure occurred during processing. </summary>
    Failure,

    /// <summary> The input provided does not meet validation requirements. </summary>
    Validation,

    /// <summary> An issue was encountered that requires attention. </summary>
    Problem,

    /// <summary> The request conflicts with the current state of the resource. </summary>
    Conflict,

    /// <summary> An internal server error occurred. </summary>
    InternalServerError
}


//[Flags]
//public enum ErrorType
//{
//    None = 0,
//    NotFound = 1 << 0,
//    BadRequest = 1 << 1,
//    Unauthorized = 1 << 2,
//    Forbidden = 1 << 3,
//    Application = 1 << 4
//}
