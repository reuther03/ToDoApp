using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Database;

namespace ToDoApp.Infrastructure.Database;

public sealed class ToDoDbContext : DbContext, IToDoDbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}