using DiskyNet.Domain.Permission.Entities;
using DiskyNet.Infrastructure.Persistence.DTOs.Permission;

namespace DiskyNet.Infrastructure.Persistence.Mappers.Permission
{
    public static class PermissionRepositoryMapper
    {
        public static RolePermissionEntity ToDomain(RolePermissionDto dto)
        {
            return RolePermissionEntity.Reconstitute(
                id: dto.Id,
                roleId: dto.RoleId,
                menuId: dto.MenuId,
                actionId: dto.ActionId,
                menuCode: dto.MenuCode,
                actionCode: dto.ActionCode
            );
        }
    }
}
