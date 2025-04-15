using HybridTodo.Api.Errors;
using HybridTodo.Api.Extensions;
using HybridTodo.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HybridTodo.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", Login).WithTags(Tags.Auth);
        app.MapPost("/api/auth/refresh", RefreshToken).WithTags(Tags.Auth);

        return app;
    }

    public static Results<SignInHttpResult, ProblemHttpResult> Login(LoginRequest request)
    {
        if (request.Email != "test@test.com" || request.Password != "Password1234")
        {
            return TypedResults.Problem(AuthError.LoginFailed.ToProblemDetails());
        }

        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, request.Email),
            new Claim(ClaimTypes.Name, request.Email),
            new Claim(ClaimTypes.Email, request.Email),
        ];

        var properties = new AuthenticationProperties();
        // Store the external provider name so we can do remote sign out
        properties.SetString(JwtBearerDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme);

        var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        return TypedResults.SignIn(principal, properties: properties, authenticationScheme: JwtBearerDefaults.AuthenticationScheme);
    }

    public static Results<SignInHttpResult, ProblemHttpResult> RefreshToken(RefreshRequest request, [FromServices] IOptionsMonitor<BearerTokenOptions> bearerTokenOptions, [FromServices] TimeProvider timeProvider)
    {
        var refreshTokenProtector = bearerTokenOptions.Get(JwtBearerDefaults.AuthenticationScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(request.RefreshToken);

        // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            timeProvider.GetUtcNow() >= expiresUtc /* Add an additional check using refreshTicket.Principal to see if the token belongs to a user*/
            )
        {
            return TypedResults.Problem(AuthError.InvalidRefreshToken.ToProblemDetails());
        }

        return TypedResults.SignIn(refreshTicket.Principal, authenticationScheme: JwtBearerDefaults.AuthenticationScheme);
    }
}
