using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Domain.TaskGroup;
using ToDoApp.Domain.User;

namespace ToDoApp.Infrastructure.Database.Configurations;

public class TaskGroupConfiguration : IEntityTypeConfiguration<TaskGroup>
{
    public void Configure(EntityTypeBuilder<TaskGroup> builder)
    {
        // Konfiguracja encji TaskGroup w Entity Framework Core
        builder.ToTable("TaskGroups");

        // Ustawienie klucza głównego i właściwości
        builder.HasKey(g => g.Id);
        // Identyfikator grupy zadań jest typu Guid i nie jest generowany automatycznie
        builder.Property(g => g.Id)
            .ValueGeneratedNever();

        // Konfiguracja nazwy grupy zadań
        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Konfiguracja kategorii grupy zadań
        builder.Property(g => g.Category)
            .HasConversion<string>()
            .IsRequired();

        // Konfiguracja właściciela grupy zadań
        builder.Property(g => g.OwnerId)
            .HasConversion(x => x.Value, x => new UserId(x))
            .ValueGeneratedNever()
            .IsRequired();

        // Ustawienie relacji z użytkownikiem
        // jedna grupa moze miec wiele zadań, ale zadanie należy do jednej grupy
        builder.HasMany(g => g.Tasks)
            .WithOne()
            .HasForeignKey("TaskGroupId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}