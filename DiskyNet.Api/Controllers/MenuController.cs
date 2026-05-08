using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiskyNet.Api.Controllers
{
    [ApiController]
    [Route("api/menu")]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMenusByUser(CancellationToken cancellationToken)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new { message = "No se pudo obtener el usuario." });

            var menus = await _menuService.GetMenusByUserAsync(userId, cancellationToken);
            return Ok(menus);
        }
    }
}
