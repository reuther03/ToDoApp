using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Features.Commands.AddGroup;
using ToDoApp.Application.Features.Commands.AddTask;
using ToDoApp.Application.Features.Commands.DeleteTask;

namespace ToDoApp.Api.Controllers;

public class ToDoController : ControllerBase
{
    private readonly ISender _sender;

    public ToDoController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("tasks")]
    [Authorize]
    public async Task<IActionResult> AddTask([FromBody] AddTaskCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("group")]
    [Authorize]
    public async Task<IActionResult> AddGroup([FromBody] AddGroupCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }


    [HttpDelete("tasks/{taskId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteTask(Guid taskId, [FromQuery] Guid groupId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteTaskCommand(taskId, groupId);
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }
}