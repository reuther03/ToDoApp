using ToDoApp.Domain.TaskGroup;

namespace ToDoApp.Application.Dto;

// TaskGroupDto reprezentuje grupę zadań w aplikacji ToDoApp jako Dto
public class TaskGroupDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Category { get; init; } = null!;
    public List<ToDoTaskDto> Tasks { get; init; } = [];
    public Guid OwnerId { get; init; }

    public static TaskGroupDto AsDto(TaskGroup group)
    {
        return new TaskGroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Category = group.Category.ToString(),
            Tasks = group.Tasks.Select(ToDoTaskDto.AsDto).ToList(),
            OwnerId = group.OwnerId.Value
        };
    }
}