using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Features.Commands.AddTask;

namespace ToDoApp.Api.Controllers;

public class ToDoTaskController : ControllerBase
{
    private readonly ISender _sender;

    public ToDoTaskController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("tasks")]
    public async Task<IActionResult> AddTask([FromBody] AddTaskCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }
}