using Microsoft.AspNetCore.Mvc;
using ToDoApp.Common.Primitives.Envelopes;

namespace ToDoApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult()
    {
        return Ok(new Envelope
        {
            StatusCode = 200,
            Data = new EmptyData()
        });
    }

    protected IActionResult HandleResult<T>(T result)
    {
        return Ok(new Envelope
        {
            StatusCode = 200,
            Data = result
        });
    }
}

public sealed class EmptyData;