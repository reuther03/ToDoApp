using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Domain.ToDoTask;

namespace ToDoApp.Infrastructure.Database.Configurations;

public class ToDoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
{
    public void Configure(EntityTypeBuilder<ToDoTask> builder)
    {
        // Konfiguracja encji ToDoTask w Entity Framework Core
        builder.ToTable("ToDoTasks");

        // Ustawienie klucza głównego i właściwości
        builder.HasKey(x => x.Id);
        // Identyfikator zadania jest typu Guid i  nie jest generowany automatycznie
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        // Konfiguracja właściwości zadania
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        // Opis zadania
        builder.Property(x => x.Description)
            .HasMaxLength(200);

        // Konfiguracja stanu ukończenia zadania
        builder.Property(x => x.IsCompleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Konfiguracja daty utworzenia i ukończenia zadania
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CompletedAt)
            .IsRequired(false);

        // Konfiguracja relacji z użytkownikiem
        // Jeden użytkownik może mieć wiele zadań, ale zadanie należy do jednego użytkownika
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}