using DiskyNet.Application.Permission.Request;
using DiskyNet.Application.Role.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiskyNet.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionManagementService _permissionService;

        public RoleController(
            IRoleService roleService,
            IPermissionManagementService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }


        [HttpGet]
        //[RequirePermission("ROLES", "VIEW")]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAllRolesAsync(cancellationToken);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        //[RequirePermission("ROLES", "VIEW")]
        public async Task<IActionResult> GetRoleById(int id, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetRoleByIdAsync(id, cancellationToken);

            if (role == null)
                return NotFound(new { message = $"No se encontró el rol con ID {id}" });

            return Ok(role);
        }

        [HttpPost]
        //[RequirePermission("ROLES", "CREATE")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var roleId = await _roleService.CreateRoleAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetRoleById), new { id = roleId }, new { id = roleId });
        }

        [HttpPut("{id}")]
        //[RequirePermission("ROLES", "EDIT")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
        {
            var updated = await _roleService.UpdateRoleAsync(id, request, cancellationToken);

            if (!updated)
                return NotFound(new { message = $"No se encontró el rol con ID {id}" });

            return Ok(new { message = "Rol actualizado exitosamente" });
        }

        [HttpGet("{id}/permissions")]
        //[RequirePermission("ROLES", "VIEW")]
        public async Task<IActionResult> GetRolePermissions(int id, CancellationToken cancellationToken)
        {
            var result = await _permissionService.GetRolePermissionsForManagementAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}/permissions")]
        //[RequirePermission("ROLES", "EDIT")]
        public async Task<IActionResult> UpdateRolePermissions(
            int id,
            [FromBody] UpdateRolePermissionsRequest request,
            CancellationToken cancellationToken)
        {
            await _permissionService.UpdateRolePermissionsAsync(id, request, cancellationToken);
            return NoContent();
        }
    }
}