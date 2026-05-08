using DiskyNet.Domain.Permission.Entities;

namespace DiskyNet.Domain.Permission.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<RolePermissionEntity>> GetRolePermissionsAsync(int roleId, CancellationToken cancellationToken);

        Task<IEnumerable<ActionEntity>> GetAllActionsAsync(CancellationToken cancellationToken);

        Task<IEnumerable<ActionEntity>> GetMenuActionsAsync(int menuId, CancellationToken cancellationToken);

        Task<Dictionary<int, List<int>>> GetAllMenuActionsAsync(CancellationToken cancellationToken);

        Task CreateRolePermissionAsync(int roleId, int menuId, int actionId, CancellationToken cancellationToken);

        Task DeleteRolePermissionAsync(int roleId, int menuId, int actionId, CancellationToken cancellationToken);

        Task DeleteAllRolePermissionsAsync(int roleId, CancellationToken cancellationToken);
    }
}
