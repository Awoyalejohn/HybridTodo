namespace HybridTodo.Shared.DTOs;

public class LoginResponse
{
    public string TokenType { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; } = default!;
}
