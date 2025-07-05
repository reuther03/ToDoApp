using ToDoApp.Common.Primitives.Domain;
using ToDoApp.Domain.User;

namespace ToDoApp.Domain.ToDoTask;

public class ToDoTask : Entity<Guid>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public User.User User { get; private set; }
    public UserId UserId { get; private set; }

    private ToDoTask()
    {
    }

    private ToDoTask(Guid id, string title, string description, UserId userId)
        : base(id)
    {
        Title = title;
        Description = description;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
        CompletedAt = null;
        UserId = userId;
    }

    public static ToDoTask Create(string title, string description, UserId userId) =>
        new(Guid.NewGuid(), title, description, userId);
}