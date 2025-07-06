using System.Text.Json.Serialization;
using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Common.Abstractions;

namespace ToDoApp.Application.Features.Commands.UpdateTask;

public record UpdateTaskCommand(
    [property: JsonIgnore]
    Guid TaskId,
    Guid GroupId,
    string Title,
    string Description) : ICommand<bool>
{
    internal sealed class Handler : ICommandHandler<UpdateTaskCommand, bool>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public Handler(IToDoRepository toDoRepository, IUserContext userContext, IUserRepository userRepository)
        {
            _toDoRepository = toDoRepository;
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId!, cancellationToken);
            if (user is null)
                throw new InvalidOperationException("User not found");

            var group = await _toDoRepository.GetGroupByIdAsync(request.GroupId, user.Id, cancellationToken);
            if (group is null)
                throw new InvalidOperationException("Group not found");

            var task = group.Tasks.FirstOrDefault(t => t.Id == request.TaskId);
            if (task is null)
                throw new InvalidOperationException("Task not found");

            task.Update(request.Title, request.Description);
            await _toDoRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}