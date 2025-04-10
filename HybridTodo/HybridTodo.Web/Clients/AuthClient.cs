using HybridTodo.Api.DTOs;
using HybridTodo.Shared.Clients;
using Microsoft.JSInterop;

namespace HybridTodo.Web.Clients;

public class AuthClient : IAuthClient
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jSRuntime;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthClient(HttpClient httpClient, IConfiguration configuration, IJSRuntime jSRuntime, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["BaseAddress"] ?? throw new ArgumentNullException("BaseAddress"));
        _jSRuntime = jSRuntime;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        if ((_httpContextAccessor?.HttpContext?.WebSockets.IsWebSocketRequest) == true) // Running inside Browser
        {
            var authModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
            bool result = await authModule.InvokeAsync<bool>("loginAsync", request.Email, request.Password);
            return result;
        }
        else // Running from Endpoint
        {
            return await Task.FromResult(true);
        }
    }

    public async Task LogoutAsync()
    {
        if ((_httpContextAccessor?.HttpContext?.WebSockets.IsWebSocketRequest) == true) // Running inside Browser
        {
            var authModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
            await authModule.InvokeVoidAsync("logoutAsync");
        }
        else // Running from Endpoint
        {

            await Task.CompletedTask;
        }
    }
}
