using ToDoApp.Common.Primitives.Domain;

namespace ToDoApp.Domain.ToDoTask;

public class ToDoTask : Entity<Guid>
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private ToDoTask()
    {
    }

    private ToDoTask(Guid id, string title, string description, bool isCompleted, DateTime createdAt, DateTime? completedAt = null)
        : base(id)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = isCompleted;
        CreatedAt = createdAt;
        CompletedAt = completedAt;
    }

    public static ToDoTask Create(string title, string description)
        => new ToDoTask(Guid.NewGuid(), title, description, false, DateTime.UtcNow);
}