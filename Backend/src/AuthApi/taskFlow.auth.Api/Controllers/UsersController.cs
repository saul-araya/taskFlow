using Microsoft.AspNetCore.Mvc;
using taskFlow.auth.Application.Interfaces;

namespace taskFlow.auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
    IUserService _service    
) : ControllerBase
{

    [HttpGet("{id}", Name = "GetUserByIdRoute")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        return Ok(await _service.FindUserByIdAsync(id));
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        return Ok(await _service.FindUserByEmailAsync(email));
    }
}
