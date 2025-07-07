using Microsoft.AspNetCore.Mvc;
using ToDoApp.Common.Primitives.Envelopes;

namespace ToDoApp.Api.Controllers;
/// <summary>
/// controller bazowy dla wszystkich kontrolerów w aplikacji.
/// tworzy zapewnić jednolitą obsługę odpowiedzi HTTP.
/// </summary>
[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    // metoda HandleResult bez parametrów zwraca pustą odpowiedź z kodem 200.
    protected IActionResult HandleResult()
    {
        return Ok(new Envelope
        {
            StatusCode = 200,
            Data = new EmptyData()
        });
    }

    // metoda HandleResult z parametrem result zwraca odpowiedź z kodem 200 i danymi.
    // jednolita odpowiedź jest opakowana w obiekt Envelope, który zawiera status kodu i dane.
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