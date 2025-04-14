using HybridTodo.Shared.Abstractions;

namespace HybridTodo.Api.Errors;

public static class AuthError
{
    public static readonly Error LoginFailed = Error.BadRequest(
        "Auth.LoginFailed",
        "Login failed, please check your email address and password"
    );
}
