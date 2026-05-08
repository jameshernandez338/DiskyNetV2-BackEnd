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
            var menus = await _menuService.GetMenusByUserAsync(cancellationToken);
            return Ok(menus);
        }
    }
}
