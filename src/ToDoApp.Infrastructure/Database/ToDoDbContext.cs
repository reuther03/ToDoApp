using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Database;
using ToDoApp.Domain.TaskGroup;
using ToDoApp.Domain.ToDoTask;
using ToDoApp.Domain.User;

namespace ToDoApp.Infrastructure.Database;

public sealed class ToDoDbContext : DbContext, IToDoDbContext
{
    // Definicje DbSet dla encji w bazie danych
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskGroup> TaskGroups => Set<TaskGroup>();

    public DbSet<ToDoTask> ToDoItems => Set<ToDoTask>();
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

}