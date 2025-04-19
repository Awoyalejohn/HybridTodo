using HybridTodo.Shared.Constants;

namespace HybridTodo.Web;

public class CookieAppendingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieAppendingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

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
