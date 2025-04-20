using HybridTodo.Abstractions.Services;
using HybridTodo.Shared.Constants;

namespace HybridTodo;

internal class TokenAppendingHandler : DelegatingHandler
{
    private readonly ITokenStorageService _tokenStorageService;

    public TokenAppendingHandler(ITokenStorageService tokenStorageService)
    {
        _tokenStorageService = tokenStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessTokenInfo = await _tokenStorageService.GetTokenFromSecureStorageAsync();

        if (accessTokenInfo != null)
        {
            request.Headers.Authorization = new(AuthConstants.Bearer, accessTokenInfo.LoginResponse.AccessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
