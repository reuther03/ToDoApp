using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Features.Commands.Login;
using ToDoApp.Application.Features.Commands.SignUp;

namespace ToDoApp.Api.Controllers;

/// <summary>
/// UserController
///  kontroler odpowiedzialny za zarządzanie użytkownikami.
/// </summary>
public class UserController : BaseController
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    // metoda SignUp rejestruje nowego użytkownika.
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    // metoda Login loguje użytkownika i zwraca token JWT.
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken = default)
    {
        var token = await _sender.Send(command, cancellationToken);
        return HandleResult(token);
    }
}