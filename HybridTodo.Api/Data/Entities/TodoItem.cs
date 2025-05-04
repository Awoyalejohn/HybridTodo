namespace HybridTodo.Api.Data.Entities;

public partial class TodoItem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool Completed { get; set; }

    public int TodoListId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public virtual TodoList TodoList { get; set; } = null!;
}
