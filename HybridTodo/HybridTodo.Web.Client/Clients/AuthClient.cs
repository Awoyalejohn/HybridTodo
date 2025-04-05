using HybridTodo.Shared.Clients;
using Microsoft.JSInterop;

namespace HybridTodo.Web.Client.Clients;

public class AuthClient : IAuthClient
{
    private readonly IJSRuntime _jSRuntime;

    public AuthClient(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    public async Task<bool> LoginAsync()
    {
        var authModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
        bool result = await authModule.InvokeAsync<bool>("loginAsync", "test@test.com", "MyPassword");
        return result;
    }

    public async Task LogoutAsync()
    {
        var authModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
        await authModule.InvokeVoidAsync("logoutAsync");
    }
}
