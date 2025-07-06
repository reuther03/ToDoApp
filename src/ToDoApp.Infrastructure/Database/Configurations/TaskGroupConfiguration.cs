using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Domain.TaskGroup;
using ToDoApp.Domain.User;

namespace ToDoApp.Infrastructure.Database.Configurations;

public class TaskGroupConfiguration : IEntityTypeConfiguration<TaskGroup>
{
    public void Configure(EntityTypeBuilder<TaskGroup> builder)
    {
        builder.ToTable("TaskGroups");

        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id)
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Category)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(g => g.OwnerId)
            .HasConversion(x => x.Value, x => new UserId(x))
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasMany(g => g.Tasks)
            .WithOne()
            .HasForeignKey("TaskGroupId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}