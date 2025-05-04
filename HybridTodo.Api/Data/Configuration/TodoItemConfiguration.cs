using HybridTodo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HybridTodo.Api.Data.Configuration;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");

        builder.Property(e => e.Completed)
            .HasDefaultValue(false);

        builder.HasOne(d => d.TodoList)
            .WithMany(p => p.TodoItems)
            .HasForeignKey(d => d.TodoListId);
    }
}
