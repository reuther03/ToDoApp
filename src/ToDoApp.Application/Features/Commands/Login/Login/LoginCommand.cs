using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Application.Dto;
using ToDoApp.Common.Abstractions;

namespace ToDoApp.Application.Features.Commands.Login.Login;

public record LoginCommand(string Email, string Password) : ICommand<AccessToken>
{
    internal sealed class Handler : ICommandHandler<LoginCommand, AccessToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public Handler(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<AccessToken> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email)
                ?? throw new InvalidOperationException($"User with email {request.Email} not found");

            if (!user.Password.Verify(request.Password))
                throw new InvalidOperationException("Invalid password");

            return AccessToken.Create(user, _jwtProvider.Generate(user));
        }
    }
}