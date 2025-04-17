namespace HybridTodo.Shared.Clients;

public interface IUserClient
{
    Task<string[]> TestAsync();
}
