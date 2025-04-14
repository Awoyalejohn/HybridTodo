using HybridTodo.Shared.Abstractions;
using HybridTodo.Shared.DTOs;

namespace HybridTodo.Shared.Clients;

public interface IAuthClient
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}
