using HybridTodo.Shared.Abstractions;

namespace HybridTodo.Api.Errors;

public static class AuthError
{
    public static readonly Error LoginFailed = Error.BadRequest(
        "Auth.LoginFailed",
        "Login failed, please check your email address and password."
    );

    public static readonly Error InvalidRefreshToken = Error.BadRequest(
        "Auth.InvalidRefreshToken",
        "Invalid refresh token, please check your refrsh token is valid."
);
}
