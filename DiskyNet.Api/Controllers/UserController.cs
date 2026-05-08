using DiskyNet.Application.User.Interfaces;
using DiskyNet.Application.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiskyNet.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        //[RequirePermission("USERS", "VIEW")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllUsersAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id}")]
        //[RequirePermission("USERS", "VIEW")]
        public async Task<IActionResult> GetUserById(long id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(user);
        }


        [HttpPost]
        //[RequirePermission("USERS", "CREATE")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var userId = await _userService.CreateUserAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { id = userId });
        }


        [HttpPut("{id}")]
        //[RequirePermission("USERS", "EDIT")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(id, request, cancellationToken);
            return NoContent();
        }
    }
}
