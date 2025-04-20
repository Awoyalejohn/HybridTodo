namespace HybridTodo.Shared.Abstractions.Clients;

public interface ITodoClient
{
    Task<string[]> TestAsync();
}
