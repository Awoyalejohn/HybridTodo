using Microsoft.AspNetCore.Http.HttpResults;
using HybridTodo.Shared.Constants;

namespace HybridTodo.Api.Endpoints;

public static class TestAuthEndpoints
{
    public static IEndpointRouteBuilder MapTestAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/test-auth", Test).WithTags(Tags.TestAuth).RequireAuthorization();
        return app;
    }

    public static Ok<string> Test() => TypedResults.Ok("Succes you are authenticated!");
}
