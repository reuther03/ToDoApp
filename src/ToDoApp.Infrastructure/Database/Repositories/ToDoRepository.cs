using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Domain.TaskGroup;
using ToDoApp.Domain.ToDoTask;
using ToDoApp.Domain.User;

namespace ToDoApp.Infrastructure.Database.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly ToDoDbContext _context;

    public ToDoRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task<TaskGroup?> GetGroupByIdAsync(Guid groupId, UserId userId, CancellationToken cancellationToken = default)
        => await _context.TaskGroups
            .Include(g => g.Tasks)
            .FirstOrDefaultAsync(g => g.Id == groupId && g.OwnerId == userId, cancellationToken);

    public async Task RemoveTaskAsync(Guid taskId, UserId userId, CancellationToken cancellationToken = default)
    {
        var task = await _context.ToDoItems
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId, cancellationToken);

        if (task is null)
            return;

        _context.ToDoItems.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task AddTaskAsync(ToDoTask toDoTask, CancellationToken cancellationToken = default)
    {
        _context.ToDoItems.Add(toDoTask);
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task AddGroupAsync(TaskGroup taskGroup, CancellationToken cancellationToken = default)
    {
        _context.TaskGroups.Add(taskGroup);
        return _context.SaveChangesAsync(cancellationToken);
    }
}