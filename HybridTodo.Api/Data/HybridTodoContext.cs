using HybridTodo.Api.Data.Configuration;
using HybridTodo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HybridTodo.Api.Data;

public partial class HybridTodoContext : DbContext
{
    public HybridTodoContext()
    {
    }

    public HybridTodoContext(DbContextOptions<HybridTodoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TodoItem> TodoItems { get; set; }

    public virtual DbSet<TodoList> TodoLists { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new TodoItemConfiguration().Configure(modelBuilder.Entity<TodoItem>());
        new TodoListConfiguration().Configure(modelBuilder.Entity<TodoList>());
        new UserConfiguration().Configure(modelBuilder.Entity<User>());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
