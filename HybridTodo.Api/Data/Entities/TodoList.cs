namespace HybridTodo.Api.Data.Entities;

public partial class TodoList
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool Completed { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

    public virtual User User { get; set; } = null!;
}
