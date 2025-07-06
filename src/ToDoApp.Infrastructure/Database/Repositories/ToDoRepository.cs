using ToDoApp.Application.Database.Repositories;
using ToDoApp.Domain.ToDoTask;

namespace ToDoApp.Infrastructure.Database.Repositories;

public class ToDoTaskRepository : IToDoTaskRepository
{
    private readonly ToDoDbContext _context;

    public ToDoTaskRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(ToDoTask toDoTask, CancellationToken cancellationToken = default)
    {
        _context.ToDoItems.Add(toDoTask);
        return _context.SaveChangesAsync(cancellationToken);
    }
}