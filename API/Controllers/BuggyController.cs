using System;
using System.Security.Claims;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("not-found")]
    public ActionResult GetNotFound()
    {
        return NotFound(new Errors.ApiErrorResponse(404, "Resource not found", null));
    }

    [HttpGet("internalerror")]
    public ActionResult GetInternalError()
    {
        throw new Exception("This is a server error");
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new Errors.ApiErrorResponse(400, "This is a bad request", null));
    }

    [HttpGet("validationerror")]
    public ActionResult GetValidationError(CreateProductDto productDto)

    {
        return Ok();
    }

    [HttpGet("unauthorized")]
    public ActionResult GetUnauthorized()
    {
        return Unauthorized(new Errors.ApiErrorResponse(401, "This is an unauthorized request", null));
    }

    [Authorize]
    [HttpGet("secret")]
    public IActionResult GetSecret()
    {
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok("Hello " + name + " com o ID " + id);
    }
}
