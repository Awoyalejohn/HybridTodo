using HybridTodo.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;

namespace HybridTodo.Web.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(this IEndpointRouteBuilder routes)
    {
        // The todo API translates the authentication cookie between the browser the BFF into an 
        // access token that is sent to the todo API. We're using YARP to forward the request.

        var group = routes.MapGroup("/api/todos");

        //group.RequireAuthorization();

        group.MapForwarder("{*path}", "https://localhost:7262", new ForwarderRequestConfig(), b =>
        {
            b.AddRequestTransform(async c =>
            {
                var accessToken = await c.HttpContext.GetTokenAsync(AuthConstants.AccessToken);
                c.ProxyRequest.Headers.Authorization = new("Bearer", accessToken);
            });
        });

        return group;
    }
}
