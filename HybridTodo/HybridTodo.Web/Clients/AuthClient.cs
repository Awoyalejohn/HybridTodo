using HybridTodo.Api.DTOs;
using HybridTodo.Shared.Clients;

namespace HybridTodo.Web.Clients;

public class AuthClient : IAuthClient
{
    private readonly HttpClient _httpClient;

    public AuthClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["BaseAddress"] ?? throw new ArgumentNullException("BaseAddress"));
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        return await Task.FromResult(true);
    }

    public async Task LogoutAsync()
    {
        await Task.CompletedTask;
    }
}
