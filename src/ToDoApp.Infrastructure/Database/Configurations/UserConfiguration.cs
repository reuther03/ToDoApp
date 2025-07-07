using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Domain.User;

namespace ToDoApp.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Konfiguracja encji User w Entity Framework Core
        builder.ToTable("Users");

        // Ustawienie klucza głównego i właściwości
        builder.HasKey(u => u.Id);
        // Identyfikator użytkownika jest typu UserId i nie jest generowany automatycznie
        builder.Property(u => u.Id)
            .HasConversion(x => x.Value, x => UserId.From(x))
            .ValueGeneratedNever();

        // Konfiguracja właściwości użytkownika
        builder.Property(u => u.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .IsRequired()
            .HasMaxLength(256);

        // Konfiguracja nazwy użytkownika
        builder.Property(u => u.Username)
            .HasConversion(x => x.Value, x => new Username(x))
            .IsRequired()
            .HasMaxLength(70);

        // Konfiguracja hasła użytkownika
        builder.Property(u => u.Password)
            .HasConversion(x => x.Value, x => new Password(x))
            .IsRequired()
            .HasMaxLength(256);

        // Ustawienie indeksu unikalnego dla adresu e-mail
        builder.HasIndex(u => u.Email).IsUnique();
    }
}