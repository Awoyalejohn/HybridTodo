using HybridTodo.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;

namespace HybridTodo.Web.Endpoints;

public static class ForwardingEndpoints
{
    public static RouteGroupBuilder MapForwardingEndpoints(this IEndpointRouteBuilder routes, IConfiguration configuration)
    {
        // The todo API translates the authentication cookie between the browser the BFF into an 
        // access token that is sent to the todo API. We're using YARP to forward the request.

        var group = routes.MapGroup("/api");

        group.RequireAuthorization();

        group.MapForwarder("{*path}", configuration["HybridTodoApiUrl"] ?? throw new ArgumentNullException("HybridTodoApiUrl"), new ForwarderRequestConfig(), b =>
        {
            b.AddRequestTransform(async c =>
            {
                var accessToken = await c.HttpContext.GetTokenAsync(AuthConstants.AccessToken);
                c.ProxyRequest.Headers.Authorization = new(AuthConstants.Bearer, accessToken);
            });
        });

        return group;
    }
}
