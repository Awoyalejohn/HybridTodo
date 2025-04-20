using HybridTodo.Shared.Abstractions;
using HybridTodo.Shared.Abstractions.Clients;
using HybridTodo.Shared.DTOs;
using Microsoft.JSInterop;

namespace HybridTodo.Web.Clients;

public class AuthClient : IAuthClient
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jSRuntime;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthClient(HttpClient httpClient, IJSRuntime jSRuntime, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _jSRuntime = jSRuntime;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        if ((_httpContextAccessor?.HttpContext?.WebSockets.IsWebSocketRequest) == true) // Running inside Browser
        {// Uses JavaScript to call the Login endpoint because SignalR/WebSockets cant set or remove cookies.
            var authModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
            var result = await authModule.InvokeAsync<Result<LoginResponse>>("loginAsync", request.Email, request.Password);
            return result;
        }
        else // Running from Endpoint
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result;
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return Result.Failure<LoginResponse>(result?.ToError() ?? Error.NullValue);
            }
        }
    }

    public async Task LogoutAsync()
    {// Uses JavaScript to call the Logout endpoint because SignalR/WebSockets cant set or remove cookies.
        var authModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
        await authModule.InvokeVoidAsync("logoutAsync");
    }

    public Task<Result<LoginResponse>> RefreshAccessTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
