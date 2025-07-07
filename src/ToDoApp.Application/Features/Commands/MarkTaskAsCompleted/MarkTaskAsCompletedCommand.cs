using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Common.Abstractions;
using ToDoApp.Domain.TaskGroup;

namespace ToDoApp.Application.Features.Commands.MarkTaskAsCompleted;

public record MarkTaskAsCompletedCommand(Guid GroupId, Guid TaskId) : ICommand<bool>
{
    internal sealed class Handler : ICommandHandler<MarkTaskAsCompletedCommand, bool>
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

        public async Task<bool> Handle(MarkTaskAsCompletedCommand request, CancellationToken cancellationToken)
        {
            // Sprawdzenie, czy użytkownik jest zalogowany
            var user = await _userRepository.GetByIdAsync(_userContext.UserId!, cancellationToken);
            if (user is null)
                throw new InvalidOperationException("User not found");

            // Sprawdzenie, czy grupa zadań istnieje i czy użytkownik ma do niej dostęp
            var group = await _toDoRepository.GetGroupByIdAsync(request.GroupId, user.Id, cancellationToken);
            if (group is null)
                throw new InvalidOperationException("Task group not found");

            // Sprawdzenie, czy zadanie istnieje w grupie
            var task = group.Tasks.FirstOrDefault(t => t.Id == request.TaskId);
            if (task is null)
                throw new InvalidOperationException("Task not found in the group");

            // Zmiana statusu zadania
            task.MarkAsCompleted();
            // Aktualizacja grupy w repozytorium
            await _toDoRepository.SaveChangesAsync(cancellationToken);

            // Zwrócenie informacji o powodzeniu operacji
            return true;
        }
    }
}