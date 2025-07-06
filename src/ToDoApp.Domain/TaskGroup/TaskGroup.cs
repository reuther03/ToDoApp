using ToDoApp.Common.Primitives.Domain;
using ToDoApp.Domain.User;

namespace ToDoApp.Domain.TaskGroup;

public class TaskGroup : Entity<Guid>
{
    private readonly List<ToDoTask.ToDoTask> _tasks = [];

    public string Name { get; private set; }
    public GroupCategory Category { get; private set; }

    public IReadOnlyList<ToDoTask.ToDoTask> Tasks => _tasks;
    public UserId OwnerId { get; private set; }

    private TaskGroup()
    {
    }

    private TaskGroup(Guid id, string name, GroupCategory category, UserId ownerId)
        : base(id)
    {
        Name = name;
        Category = category;
        OwnerId = ownerId;
    }

    public static TaskGroup Create(string name, GroupCategory category, UserId ownerId) =>
        new(Guid.NewGuid(), name, category, ownerId);

    public void AddTask(ToDoTask.ToDoTask task)
    {
        if (_tasks.Contains(task))
            throw new InvalidOperationException("Task already exists in the group.");

        _tasks.Add(task);
    }

    public void RemoveTask(ToDoTask.ToDoTask task)
    {
        if (!_tasks.Contains(task))
            throw new InvalidOperationException("Task does not exist in the group.");

        _tasks.Remove(task);
    }
}