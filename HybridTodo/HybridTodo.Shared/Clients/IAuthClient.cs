namespace HybridTodo.Shared.Clients;

public interface IAuthClient
{
    Task<bool> LoginAsync();
    Task LogoutAsync();
}
