using HybridTodo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HybridTodo.Api.Data.Configuration;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.Property(e => e.Completed)
            .HasDefaultValue(false);

        builder.HasOne(d => d.User)
            .WithMany(p => p.TodoLists)
            .HasForeignKey(d => d.UserId);
    }
}
