using HybridTodo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HybridTodo.Api.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.FullName)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.Password)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.PasswordHash)
            .HasMaxLength(255)
            .IsUnicode(false);
    }
}
