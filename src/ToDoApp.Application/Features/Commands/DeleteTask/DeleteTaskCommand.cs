using MediatR;
using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Common.Abstractions;

namespace ToDoApp.Application.Features.Commands.DeleteTask;

public record DeleteTaskCommand(Guid TaskId, Guid GroupId) : ICommand
{
    internal sealed class Handler : ICommandHandler<DeleteTaskCommand>
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

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            if (user is null)
                throw new InvalidOperationException("User not found");

            var group = await _toDoRepository.GetGroupByIdAsync(request.GroupId, user.Id, cancellationToken);
            if (group is null)
                throw new InvalidOperationException("Task group not found or you do not have access to it");

            var task = group.Tasks.FirstOrDefault(x => x.Id == request.TaskId);
            if (task is null)
                throw new InvalidOperationException("Task not found or you do not have access to it");

            group.RemoveTask(task);
            await _toDoRepository.RemoveTaskAsync(request.TaskId, user.Id, cancellationToken);

            return Unit.Value;
        }
    }
}