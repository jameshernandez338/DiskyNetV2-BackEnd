using DiskyNet.Application.Permission.Request;
using DiskyNet.Application.Permission.Response;

namespace DiskyNet.Application.Permission.Interfaces
{
    public interface IPermissionManagementService
    {
        Task<RolePermissionsManagementResponse> GetRolePermissionsForManagementAsync(int roleId, CancellationToken cancellationToken);

        Task UpdateRolePermissionsAsync(int roleId, UpdateRolePermissionsRequest request, CancellationToken cancellationToken);
    }
}
