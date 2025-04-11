using HybridTodo.Shared.Clients;
using Microsoft.JSInterop;
using HybridTodo.Shared.DTOs;

namespace HybridTodo.Web.Clients;

public class AuthClient : IAuthClient
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jSRuntime;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthClient(HttpClient httpClient, IConfiguration configuration, IJSRuntime jSRuntime, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["HybridTodoApiUrl"] ?? throw new ArgumentNullException("HybridTodoApiUrl"));
        _jSRuntime = jSRuntime;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if ((_httpContextAccessor?.HttpContext?.WebSockets.IsWebSocketRequest) == true) // Running inside Browser
        {
            var authModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
            var result = await authModule.InvokeAsync<LoginResponse>("loginAsync", request.Email, request.Password);
            return result;
        }
        else // Running from Endpoint
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth",request);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result;
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
