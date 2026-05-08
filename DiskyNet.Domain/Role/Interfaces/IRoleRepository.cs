using DiskyNet.Domain.Role.Entities;

namespace DiskyNet.Domain.Role.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleEntity>> GetAllRolesAsync(CancellationToken cancellationToken);
        Task<RoleEntity?> GetRoleByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateRoleAsync(RoleEntity role, CancellationToken cancellationToken);
        Task<bool> UpdateRoleAsync(RoleEntity role, CancellationToken cancellationToken);
        Task<bool> ExistsNameAsync(string name, int? excludeId, CancellationToken cancellationToken);
    }
}