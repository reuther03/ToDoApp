using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Features.Commands.AddGroup;
using ToDoApp.Application.Features.Commands.AddTask;
using ToDoApp.Application.Features.Commands.DeleteTask;
using ToDoApp.Application.Features.Commands.MarkTaskAsCompleted;
using ToDoApp.Application.Features.Commands.RemoveGroup;
using ToDoApp.Application.Features.Commands.UpdateTask;

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

    [HttpPatch("tasks/mark-completed")]
    [Authorize]
    public async Task<IActionResult> MarkTaskAsCompleted([FromBody] MarkTaskAsCompletedCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("tasks/{taskId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command with { TaskId = taskId }, cancellationToken);
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

    [HttpDelete("group/{groupId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteGroup(Guid groupId, CancellationToken cancellationToken = default)
    {
        await _sender.Send(new RemoveGroupCommand(groupId), cancellationToken);
        return NoContent();
    }
}