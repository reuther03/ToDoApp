using ToDoApp.Domain.TaskGroup;
using ToDoApp.Domain.ToDoTask;
using ToDoApp.Domain.User;

namespace ToDoApp.Application.Database.Repositories;

public interface IToDoRepository
{
    Task<TaskGroup?> GetGroupByIdAsync(Guid groupId, UserId userId, CancellationToken cancellationToken = default);
    Task RemoveTaskAsync(Guid taskId, UserId userId, CancellationToken cancellationToken = default);
    Task RemoveGroupAsync(Guid groupId, UserId userId, CancellationToken cancellationToken = default);
    Task AddTaskAsync(ToDoTask toDoTask, CancellationToken cancellationToken = default);
    Task AddGroupAsync(TaskGroup taskGroup, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}