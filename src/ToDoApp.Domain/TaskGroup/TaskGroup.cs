using ToDoApp.Common.Primitives.Domain;
using ToDoApp.Domain.User;

namespace ToDoApp.Domain.TaskGroup;

public class TaskGroup : Entity<Guid>
{
    // Lista zadań w grupie
    private readonly List<ToDoTask.ToDoTask> _tasks = [];

    // Nazwa grupy zadań
    public string Name { get; private set; }

    // Kategoria grupy zadań
    public GroupCategory Category { get; private set; }

    // Lista zadań w grupie, tylko do odczytu
    public IReadOnlyList<ToDoTask.ToDoTask> Tasks => _tasks;
    // Identyfikator właściciela grupy zadań
    public UserId OwnerId { get; private set; }

    // Konstruktor prywatny
    private TaskGroup()
    {
    }

     // Konstruktor prywatny z parametrami
    private TaskGroup(Guid id, string name, GroupCategory category, UserId ownerId)
        : base(id)
    {
        Name = name;
        Category = category;
        OwnerId = ownerId;
    }

    // Metoda statyczna do tworzenia nowej grupy zadań
    public static TaskGroup Create(string name, GroupCategory category, UserId ownerId) =>
        new(Guid.NewGuid(), name, category, ownerId);

    // Metoda do dodawania zadania do grupy
    public void AddTask(ToDoTask.ToDoTask task)
    {
        if (_tasks.Contains(task))
            throw new InvalidOperationException("Task already exists in the group.");

        _tasks.Add(task);
    }

    // Metoda do usuwania zadania z grupy
    public void RemoveTask(ToDoTask.ToDoTask task)
    {
        if (!_tasks.Contains(task))
            throw new InvalidOperationException("Task does not exist in the group.");

        _tasks.Remove(task);
    }
}