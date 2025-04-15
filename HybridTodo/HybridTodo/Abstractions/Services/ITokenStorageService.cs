using HybridTodo.Shared.DTOs;

namespace HybridTodo.Abstractions.Services;

public interface ITokenStorageService
{
    Task<AccessTokenInfo?> GetTokenFromSecureStorageAsync();
    void RemoveToken();
    Task<AccessTokenInfo?> SaveTokenToSecureStorageAsync(LoginResponse loginModel);
}
