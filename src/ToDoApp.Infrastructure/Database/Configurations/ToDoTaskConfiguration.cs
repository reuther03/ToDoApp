using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Domain.ToDoTask;

namespace ToDoApp.Infrastructure.Database.Configurations;

public class ToDoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
{
    public void Configure(EntityTypeBuilder<ToDoTask> builder)
    {
        builder.ToTable("ToDoTasks");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(200);

        builder.Property(x => x.IsCompleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CompletedAt)
            .IsRequired(false);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}