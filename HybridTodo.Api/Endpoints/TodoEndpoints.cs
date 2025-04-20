using HybridTodo.Shared.Constants;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HybridTodo.Api.Endpoints;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/todos/test", Test).WithTags(Tags.Todos).RequireAuthorization();
        return app;
    }

    public static Ok<string[]> Test()
    {
        return TypedResults.Ok<string[]>(["Item1", "Item2", "Item3", "Item4", "Item5"]);
    }
}
