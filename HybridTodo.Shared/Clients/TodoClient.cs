﻿using HybridTodo.Shared.Abstractions.Clients;
using System.Net.Http.Json;


namespace HybridTodo.Shared.Clients;

public class TodoClient : ITodoClient
{
    private readonly HttpClient _httpClient;

    public TodoClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string[]> TestAsync()
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync("api/todos/test");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        var json = await response.Content.ReadAsStringAsync();
        return await response.Content.ReadFromJsonAsync<string[]>();
    }
}
