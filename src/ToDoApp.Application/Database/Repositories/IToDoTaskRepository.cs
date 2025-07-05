using ToDoApp.Domain.ToDoTask;

namespace ToDoApp.Application.Database.Repositories;

public interface IToDoTaskRepository
{
    Task AddAsync(ToDoTask toDoTask, CancellationToken cancellationToken = default);
}