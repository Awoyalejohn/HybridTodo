using HybridTodo.Shared.DTOs;

namespace HybridTodo.Shared.Clients;

public interface IAuthClient
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}
