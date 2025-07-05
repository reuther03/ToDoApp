using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Database;
using ToDoApp.Domain.ToDoTask;
using ToDoApp.Domain.User;

namespace ToDoApp.Infrastructure.Database;

public sealed class ToDoDbContext : DbContext, IToDoDbContext
{
    public DbSet<User> Users => Set<User>();
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