using HybridTodo.Shared.Abstractions;
using HybridTodo.Shared.Clients;
using HybridTodo.Shared.DTOs;
using System.Net.Http.Json;

namespace HybridTodo.Web.Client.Clients;

public class AuthClient : IAuthClient
{
    private readonly HttpClient _httpClient;

    public AuthClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        //var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        //var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        //return result;
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

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("api/auth/logout", null);
    }
}
