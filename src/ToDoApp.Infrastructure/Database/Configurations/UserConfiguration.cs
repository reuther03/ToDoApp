using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Domain.User;

namespace ToDoApp.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasConversion(x => x.Value, x => UserId.From(x))
            .ValueGeneratedNever();

        builder.Property(u => u.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Username)
            .HasConversion(x => x.Value, x => new Username(x))
            .IsRequired()
            .HasMaxLength(70);

        builder.Property(u => u.Password)
            .HasConversion(x => x.Value, x => new Password(x))
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}