using ToDoApp.Common.Primitives.Domain;
using ToDoApp.Domain.User;

namespace ToDoApp.Domain.ToDoTask;

public class ToDoTask : Entity<Guid>
{
    // nazwa zadania
    public string Title { get; private set; }

    // opis zadania
    public string Description { get; private set; }

    // czy zadanie jest ukończone
    public bool IsCompleted { get; private set; }

    // data utworzenia zadania
    public DateTime CreatedAt { get; private set; }

    // data ukończenia zadania, może być null jeśli zadanie nie jest ukończone
    public DateTime? CompletedAt { get; private set; }

    // właściciel zadania, reprezentowany przez użytkownika
    public User.User User { get; private set; }

    // identyfikator użytkownika, który jest właścicielem zadania
    public UserId UserId { get; private set; }

    // Konstruktor bezparametrowy wymagany przez Entity Framework
    private ToDoTask()
    {
    }

    // Prywatny konstruktor do tworzenia nowego zadania
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

    // Metoda statyczna do tworzenia nowego zadania
    public static ToDoTask Create(string title, string description, UserId userId) =>
        new(Guid.NewGuid(), title, description, userId);

    // Metoda do aktualizacji stanu zadania
    public void MarkAsCompleted()
    {
        if (IsCompleted)
        {
            IsCompleted = false;
            CompletedAt = null;
        }
        else
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }
    }
}