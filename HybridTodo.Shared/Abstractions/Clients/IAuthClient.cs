using HybridTodo.Shared.Abstractions;
using HybridTodo.Shared.DTOs;

namespace HybridTodo.Shared.Abstractions.Clients;

public interface IAuthClient
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task LogoutAsync();
    Task<Result<LoginResponse>> RefreshAccessTokenAsync(string refreshToken);
}
