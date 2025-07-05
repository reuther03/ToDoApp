using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Common.Abstractions;
using ToDoApp.Domain.ToDoTask;

namespace ToDoApp.Application.Features.Commands.AddTask;

public record AddTaskCommand(string Title, string Description) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddTaskCommand, Guid>
    {
        private readonly IToDoTaskRepository _toDoTaskRepository;
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public Handler(IToDoTaskRepository toDoTaskRepository, IUserContext userContext, IUserRepository userRepository)
        {
            _toDoTaskRepository = toDoTaskRepository;
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken)
                ?? throw new InvalidOperationException("User not found");

            var task = ToDoTask.Create(request.Title, request.Description, user.Id);
            await _toDoTaskRepository.AddAsync(task, cancellationToken);
            return task.Id;
        }
    }
}