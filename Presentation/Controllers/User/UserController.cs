using BichoApi.Domain.Entities.User;
using BichoApi.Domain.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BichoApi.Presentation.Controllers.User;

[Route("api/user")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<IEnumerable<UserEntity>>> GetAllUsers()
    {
        return Ok(await userService.GetAllUsers());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserEntity?>> Get(int id)
    {
        var user = await userService.GetUserById(id);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }

    [HttpPut("{id:int}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id:int}")]
    public void Delete(int id)
    {
    }
}