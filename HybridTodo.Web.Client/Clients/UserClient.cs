using HybridTodo.Shared.Abstractions.Clients;
using System.Net.Http.Json;


namespace HybridTodo.Web.Client.Clients;

public class UserClient : IUserClient
{
    private readonly HttpClient _httpClient;

    public UserClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string[]> TestAsync()
    {
        var response = await _httpClient.GetAsync("api/users/test");
        return await response.Content.ReadFromJsonAsync<string[]>();
    }
}
