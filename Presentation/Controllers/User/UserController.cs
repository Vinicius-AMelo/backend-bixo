using bixoApi.Models.User;
using Microsoft.AspNetCore.Mvc;
using bixoApi.Services.User;
using Microsoft.AspNetCore.Http.HttpResults;

namespace bixoApi.Controllers.User
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserEntity>>> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserEntity?>> Get(int id)
        {
            UserEntity? user = await _userService.GetUserById(id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserEntity user)
        {
            return Ok(await _userService.CreateUser(user));
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
}