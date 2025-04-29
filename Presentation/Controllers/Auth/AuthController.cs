using BichoApi.Domain.Entities.User;
using BichoApi.Domain.Interfaces.Auth;
using BichoApi.Presentation.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BichoApi.Presentation.Controllers.Auth;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserEntity>> Register([FromBody] RegisterDto registerDto)
    {
        UserEntity? user = await authService.CreateUser(registerDto);
        return Created("User created successfully!", user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ResponseLoginDto>> Login([FromBody] LoginDto loginDto)
    {
        string? validation = await authService.GetUserByEmail(loginDto);
        if (validation == null) return Unauthorized();
        return Ok(new ResponseLoginDto { Token = validation });
    }
}