using System.ComponentModel.DataAnnotations;

namespace DiskyNet.Application.Permission.Request
{
    public sealed record UpdateRolePermissionsRequest(
        [Required] List<RolePermissionItemRequest> Permissions
    );

    public sealed record RolePermissionItemRequest(
        [Required] int MenuId,
        [Required] int ActionId,
        [Required] bool IsGranted
    );
}
