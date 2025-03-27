using System.Net.Http.Json;

namespace HybridTodo.Shared;

public class AuthClient
{
    private readonly HttpClient _httpClient;

    public AuthClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> LoginAsync(string? email, string? password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new UserInfo { Email = email, Password = password });
        return response.IsSuccessStatusCode;
    }
}
