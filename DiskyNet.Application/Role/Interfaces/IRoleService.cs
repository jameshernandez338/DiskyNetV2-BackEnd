using DiskyNet.Application.Role.Request;
using DiskyNet.Application.Role.Response;

namespace DiskyNet.Application.Role.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync(CancellationToken cancellationToken);
        Task<RoleResponse?> GetRoleByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken);
        Task<bool> UpdateRoleAsync(int id, UpdateRoleRequest request, CancellationToken cancellationToken);
    }
}
