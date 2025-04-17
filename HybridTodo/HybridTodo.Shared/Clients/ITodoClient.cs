namespace HybridTodo.Shared.Clients;

public interface ITodoClient
{
    Task<string[]> TestAsync();
}
