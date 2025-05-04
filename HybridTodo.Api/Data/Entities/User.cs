namespace HybridTodo.Api.Data.Entities;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<TodoList> TodoLists { get; set; } = new List<TodoList>();
}
