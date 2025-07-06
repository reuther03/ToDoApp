using ToDoApp.Domain.ToDoTask;

namespace ToDoApp.Application.Dto;

public class ToDoTaskDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public bool IsCompleted { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public Guid UserId { get; init; }


    public static ToDoTaskDto AsDto(ToDoTask task)
    {
        return new ToDoTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt,
            UserId = task.UserId.Value
        };
    }
}