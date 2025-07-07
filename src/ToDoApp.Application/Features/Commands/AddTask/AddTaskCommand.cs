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
            // Sprawdzenie, czy użytkownik jest zalogowany
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            // Jeśli użytkownik nie istnieje, zgłoś wyjątek
            if (user is null)
                throw new InvalidOperationException("User not found");

            // Sprawdzenie, czy grupa zadań istnieje i czy użytkownik ma do niej dostęp
            var group = await _toDoRepository.GetGroupByIdAsync(request.GroupId, user.Id, cancellationToken);
            if (group is null)
                throw new InvalidOperationException("Task group not found or you do not have access to it");

            // Tworzenie nowego zadania
            var task = ToDoTask.Create(request.Title, request.Description, user.Id);
            // Dodanie zadania do grupy
            group.AddTask(task);

            // Dodanie zadania do repozytorium
            await _toDoRepository.AddTaskAsync(task, cancellationToken);
            // zwrócenie identyfikatora nowo utworzonego zadania
            return task.Id;
        }
    }
}