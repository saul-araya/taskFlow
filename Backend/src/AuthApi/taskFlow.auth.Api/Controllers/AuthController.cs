using Microsoft.AspNetCore.Mvc;
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Interfaces;

namespace taskFlow.auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthsController(
    IAuthService _service
) : ControllerBase
{
    [HttpPost("/local")]
    public async Task<IActionResult> LocalAuthentication([FromBody] LocalAuthRequestDto dto)
    {
        return Ok(await _service.LocalAuthenticate(dto));
    }

    [HttpPost("/google")]
    public async Task<IActionResult> GoogleAuthentication([FromBody] GoogleAuthRequestsDto dto)
    {
        return Ok(await _service.GoogleAuthenticate(dto));
    }

    [HttpPost("/logout")]
    public async Task<IActionResult> LogOut([FromBody] ReqLogOutDto dto)
    {
        await _service.LogOut(dto);
        return Ok();
    }
}
