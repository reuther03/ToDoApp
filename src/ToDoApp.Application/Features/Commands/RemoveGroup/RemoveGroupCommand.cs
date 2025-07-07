using MediatR;
using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Common.Abstractions;
using ToDoApp.Domain.TaskGroup;

namespace ToDoApp.Application.Features.Commands.RemoveGroup;

public record RemoveGroupCommand(Guid GroupId) : ICommand
{
    internal sealed class Handler : ICommandHandler<RemoveGroupCommand>
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

        public async Task<Unit> Handle(RemoveGroupCommand request, CancellationToken cancellationToken)
        {
            // Sprawdzenie, czy użytkownik jest zalogowany
            var user = await _userRepository.GetByIdAsync(_userContext.UserId!, cancellationToken);
            if (user is null)
                throw new InvalidOperationException("User not found");

            // Sprawdzenie, czy grupa zadań istnieje i czy użytkownik ma do niej dostęp
            var group = await _toDoRepository.GetGroupByIdAsync(request.GroupId, user.Id, cancellationToken);
            if (group is null)
                throw new InvalidOperationException("Group not found");

            // Usunięcie grupy z repozytorium
            await _toDoRepository.RemoveGroupAsync(group.Id, user.Id, cancellationToken);
            // Zwrócenie jednostki, ponieważ nie zwracamy żadnej wartości
            return Unit.Value;
        }
    }
}