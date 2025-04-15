using HybridTodo.Shared.Abstractions;
using HybridTodo.Shared.Clients;
using HybridTodo.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace HybridTodo.Clients;

internal sealed class AuthClient : IAuthClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthClient(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            await ((MauiAuthenticationStateProvider)_authenticationStateProvider).LogInAsync(result);
            return result;
        }
        else
        {
            var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            return Result.Failure<LoginResponse>(result?.ToError() ?? Error.NullValue);
        }
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<LoginResponse>> RefreshAccessTokenAsync(string refreshToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", new { RefreshToken = refreshToken });
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
