using HybridTodo.Shared.Clients;
using Microsoft.JSInterop;
using HybridTodo.Shared.DTOs;

namespace HybridTodo.Web.Client.Clients;

public class AuthClient : IAuthClient
{
    private readonly IJSRuntime _jSRuntime;

    public AuthClient(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var authModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
        var result = await authModule.InvokeAsync<LoginResponse>("loginAsync", request.Email, request.Password);
        return result;
    }

    public async Task LogoutAsync()
    {
        var authModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
        await authModule.InvokeVoidAsync("logoutAsync");
    }
}
