using HybridTodo.Shared.Constants;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HybridTodo.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/test", Test).WithTags(Tags.Users).RequireAuthorization();
        return app;
    }

    public static Ok<string[]> Test()
    {
        return TypedResults.Ok<string[]>(["Users1", "Users2", "Users3", "Users4", "Users5"]);
    }
}
