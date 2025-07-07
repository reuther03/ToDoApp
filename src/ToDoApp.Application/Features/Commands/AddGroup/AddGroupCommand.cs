using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Common.Abstractions;
using ToDoApp.Domain.TaskGroup;

namespace ToDoApp.Application.Features.Commands.AddGroup;

public record AddGroupCommand(GroupCategory Category, string Title) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddGroupCommand, Guid>
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

        public async Task<Guid> Handle(AddGroupCommand request, CancellationToken cancellationToken)
        {
            // Sprawdzenie, czy użytkownik jest zalogowany
            var user = await _userRepository.GetByIdAsync(_userContext.UserId!, cancellationToken);
            // Jeśli użytkownik nie istnieje, zgłoś wyjątek
            if (user is null)
                throw new InvalidOperationException("User not found");

            // tworzenie nowej grupy zadań
            var group = TaskGroup.Create(request.Title, request.Category, user.Id);

            // Dodanie grupy do repozytorium
            await _toDoRepository.AddGroupAsync(group, cancellationToken);

            // zwrócenie identyfikatora nowo utworzonej grupy
            return group.Id;
        }
    }
}