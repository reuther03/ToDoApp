using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Common.Abstractions;
using ToDoApp.Domain.ToDoTask;

namespace ToDoApp.Application.Features.Commands.AddTask;

public record AddTaskCommand(Guid GroupId, string Title, string Description) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddTaskCommand, Guid>
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

        public async Task<Guid> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            if (user is null)
                throw new InvalidOperationException("User not found");

            var group = await _toDoRepository.GetGroupByIdAsync(request.GroupId, user.Id, cancellationToken);
            if (group is null)
                throw new InvalidOperationException("Task group not found or you do not have access to it");

            var task = ToDoTask.Create(request.Title, request.Description, user.Id);
            group.AddTask(task);

            await _toDoRepository.AddTaskAsync(task, cancellationToken);
            return task.Id;
        }
    }
}