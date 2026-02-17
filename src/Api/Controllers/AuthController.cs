using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Netdemo.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Login()
    {
        // Authentication command flow will be implemented in the next increment.
        return Ok(new { message = "Auth scaffold ready." });
    }
}
