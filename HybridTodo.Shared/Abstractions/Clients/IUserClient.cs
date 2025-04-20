namespace HybridTodo.Shared.Abstractions.Clients;

public interface IUserClient
{
    Task<string[]> TestAsync();
}
