using HybridTodo.Api.DTOs;

namespace HybridTodo.Shared.Clients;

public interface IAuthClient
{
    Task<bool> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}
