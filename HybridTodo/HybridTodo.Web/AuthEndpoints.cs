using HybridTodo.Shared.Clients;
using HybridTodo.Shared.Constants;
using HybridTodo.Shared.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace HybridTodo.Web;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", Login).WithTags(Tags.Auth);
        app.MapPost("/api/auth/logout", Logout).WithTags(Tags.Auth);
        return app;
    }

    public static async Task<Results<Ok<LoginResponse>, ProblemHttpResult>> Login(LoginRequest request, HttpContext httpContext, IAuthClient authClient)
    {
        var result = await authClient.LoginAsync(request);

        if (result.IsSuccess)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, request.Email));

            var properties = new AuthenticationProperties();

            properties.StoreTokens([
                new AuthenticationToken { Name = AuthConstants.AccessToken, Value = result.Value.AccessToken }
            ]);

            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(
                scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                principal: principal,
                properties: properties
                );

            //return TypedResults.Ok(result);
            return TypedResults.Ok(result.Value);
        }
        else
        {
            return TypedResults.Problem(
                title: result.Error.Code,
                detail: result.Error.Message,
                type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                statusCode: 400 // BadRequest
            );
        }
    }

    public static SignOutHttpResult Logout() => TypedResults.SignOut(null, [CookieAuthenticationDefaults.AuthenticationScheme]);
}
