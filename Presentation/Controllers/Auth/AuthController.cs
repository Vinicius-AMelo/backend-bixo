using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Entities.User;
using BichoApi.Domain.Interfaces.User;
using BichoApi.Presentation.DTO.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BichoApi.Presentation.Controllers.Auth;

[Route("api/auth")]
[ApiController]
public class AuthController(IUserService userService) : ControllerBase
{

    [HttpPost("/register")]
    public async Task<ActionResult> Register([FromBody] AuthDto authDto)
    {
        await userService.CreateUser(authDto);
        return Ok("user");
    }
}

