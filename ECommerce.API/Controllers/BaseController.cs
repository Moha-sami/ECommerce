using ECommerce.Application.common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected ActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        var error = result.Errors[0];

        return error.ErrorType switch
        {
            ErrorType.NotFound => NotFound(result.Errors),
            ErrorType.ValidationError => BadRequest(result.Errors),
            ErrorType.Conflict => Conflict(result.Errors),
            ErrorType.Unauthorized => Unauthorized(result.Errors),
            ErrorType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, result.Errors),
            ErrorType.InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, result.Errors),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result.Errors)
        };
    }

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return HandleResult((Result)result);
    }
}
