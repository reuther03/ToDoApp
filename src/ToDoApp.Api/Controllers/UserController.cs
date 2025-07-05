using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Features.Commands.Login.Login;
using ToDoApp.Application.Features.Commands.SignUp;

namespace ToDoApp.Api.Controllers;

public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken = default)
    {
        var token = await _sender.Send(command, cancellationToken);
        return Ok(token);
    }
}