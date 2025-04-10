﻿using HybridTodo.Shared.Clients;
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
        app.MapPost("/api/auth/login", Login).WithTags("Auth");
        app.MapPost("/api/auth/logout", Logout).WithTags("Auth");
        return app;
    }

    public static async Task<Results<Ok<LoginResponse>, BadRequest>> Login(LoginRequest request, HttpContext httpContext, IAuthClient authClient)
    {
        var result = await authClient.LoginAsync(request);
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Email));
        identity.AddClaim(new Claim(ClaimTypes.Name, request.Email));

        var properties = new AuthenticationProperties();

        properties.StoreTokens([
            new AuthenticationToken { Name = AuthConstants.AccessToken, Value = result.AccessToken }
        ]);

        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(
            scheme: CookieAuthenticationDefaults.AuthenticationScheme,
            principal: principal,
            properties: properties
            );

        return TypedResults.Ok(result);
    }

    public static SignOutHttpResult Logout() => TypedResults.SignOut(null, [CookieAuthenticationDefaults.AuthenticationScheme]);
}
