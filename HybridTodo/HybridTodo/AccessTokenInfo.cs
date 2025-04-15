using HybridTodo.Shared.DTOs;

namespace HybridTodo;

public sealed class AccessTokenInfo
{
    //public required string Email { get; set; }
    public required LoginResponse LoginResponse { get; set; }
    public required DateTime AccessTokenExpiration { get; set; }
}
