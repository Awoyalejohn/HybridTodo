using HybridTodo.Api.Constants;
using HybridTodo.Api.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace HybridTodo.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth", Login).WithTags(Tags.Auth);
        return app;
    }

    public static Results<SignInHttpResult, UnauthorizedHttpResult> Login(LoginRequest request)
    {
        if (request.Email != "test@test.com" || request.Password != "Password1234")
        {
            return TypedResults.Unauthorized();
        }

        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, request.Email),
            new Claim(ClaimTypes.Name, request.Email),
            new Claim(ClaimTypes.Email, request.Email),
        ];

        var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        return TypedResults.SignIn(principal);
    }
}
