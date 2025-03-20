using System.ComponentModel.DataAnnotations;

namespace HybridTodo.Shared;

public class UserInfo
{
    [Required]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}