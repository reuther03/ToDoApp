using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.TaskGroup;
using ToDoApp.Domain.ToDoTask;
using ToDoApp.Domain.User;

namespace ToDoApp.Application.Database;

public interface IToDoDbContext
{
    DbSet<User> Users { get; }
    DbSet<TaskGroup> TaskGroups { get; }
    DbSet<ToDoTask> ToDoItems { get; }
}