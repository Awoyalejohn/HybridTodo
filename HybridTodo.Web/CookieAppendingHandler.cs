using HybridTodo.Shared.Constants;

namespace HybridTodo.Web;

public class CookieAppendingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieAppendingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // HTTP requests made during prerendering are executed by the server.
    // Since the browser hasn't yet taken over, any authentication cookies available in the user's browser are not automatically sent.
    // To ensure authenticated API requests work during prerendering, we manually attach the authentication cookie from the server-side HttpContext to the outgoing request headers.
    // Note: Once the app switches to client-side execution, cookies are handled automatically by the browser, and this logic is bypassed.
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor?.HttpContext;

        if (httpContext.Request.Cookies.TryGetValue(AuthConstants.AuthCookieName, out var cookieValue))
        {
            request.Headers.Add(AuthConstants.Cookie, $"{AuthConstants.AuthCookieName}={cookieValue}");
        }
        return base.SendAsync(request, cancellationToken);
    }
}
