using ToDoApp.Application.Database.Repositories;
using ToDoApp.Common.Abstractions;
using ToDoApp.Domain.User;
using PassValueObject = ToDoApp.Domain.User.Password;


namespace ToDoApp.Application.Features.Commands.SignUp;

public record SignUpCommand(string Email, string Username, string Password) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<SignUpCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public Handler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            // Sprawdzenie, czy użytkownik już istnieje
            if (await _userRepository.ExistsWithEmailAsync(new Email(request.Email), cancellationToken) ||
                await _userRepository.ExistsWithUsernameAsync(new Username(request.Username), cancellationToken))
            {
                throw new InvalidOperationException("username already exists");
            }

            // Tworzenie nowego użytkownika
            var user = User.Create(new Username(request.Username), new Email(request.Email), PassValueObject.Create(request.Password));

            // Dodanie użytkownika do repozytorium
            await _userRepository.AddAsync(user, cancellationToken);
            // Zwrócenie identyfikatora nowo utworzonego użytkownika
            return user.Id;
        }
    }
}