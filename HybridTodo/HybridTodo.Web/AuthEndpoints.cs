using HybridTodo.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace HybridTodo.Web;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (UserInfo userInfo, HttpContext httpContext) =>
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userInfo.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, userInfo.Email));

            var properties = new AuthenticationProperties();

            //properties.StoreTokens([
            //    new AuthenticationToken { Name = TokenNames.AccessToken, Value = token }
            //]);

            //return Results.SignIn(new ClaimsPrincipal(identity),
            //    properties: properties,
            //    authenticationScheme: CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);


            //return TypedResults.SignIn(principal);

            await httpContext.SignInAsync(principal);

            // Redirect to a new endpoint to ensure the browser processes the cookie
            return  TypedResults.Redirect("https://localhost:7175/login-success");


        }).WithTags("Auth");

        return app;
    }
}
