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

    /// Pobiera wszystkie zadania użytkownika
    public async Task<TaskGroup?> GetGroupByIdAsync(Guid groupId, UserId userId, CancellationToken cancellationToken = default)
        => await _context.TaskGroups
            .Include(g => g.Tasks)
            .FirstOrDefaultAsync(g => g.Id == groupId && g.OwnerId == userId, cancellationToken);

    /// Pobiera wszystkie zadania użytkownika
    public async Task RemoveTaskAsync(Guid taskId, UserId userId, CancellationToken cancellationToken = default)
    {
        var task = await _context.ToDoItems
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId, cancellationToken);

        if (task is null)
            return;

        _context.ToDoItems.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // Usuwa grupę zadań, jeśli należy do użytkownika
    public async Task RemoveGroupAsync(Guid groupId, UserId userId, CancellationToken cancellationToken = default)
    {
        var group = await _context.TaskGroups
            .FirstOrDefaultAsync(g => g.Id == groupId && g.OwnerId == userId, cancellationToken);

        if (group is null)
            return;

        _context.TaskGroups.Remove(group);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // Dodaje nowe zadanie do repozytorium
    public Task AddTaskAsync(ToDoTask toDoTask, CancellationToken cancellationToken = default)
    {
        _context.ToDoItems.Add(toDoTask);
        return _context.SaveChangesAsync(cancellationToken);
    }

    // Dodaje nową grupę zadań do repozytorium
    public Task AddGroupAsync(TaskGroup taskGroup, CancellationToken cancellationToken = default)
    {
        _context.TaskGroups.Add(taskGroup);
        return _context.SaveChangesAsync(cancellationToken);
    }

    // saves changes bazy danych
    public async Task SaveChangesAsync(CancellationToken c = default)
    {
        await _context.SaveChangesAsync(c);
    }
}