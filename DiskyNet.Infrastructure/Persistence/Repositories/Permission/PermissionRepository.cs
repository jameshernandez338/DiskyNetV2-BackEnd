using DiskyNet.Domain.Permission.Entities;
using DiskyNet.Domain.Permission.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.Permission;
using DiskyNet.Infrastructure.Persistence.ErrorHandling;
using DiskyNet.Infrastructure.Persistence.Mappers.Permission;
using Microsoft.Data.SqlClient;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Permission
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public PermissionRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<RolePermissionEntity>> GetRolePermissionsAsync(int roleId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    rp.Id,
                    rp.RoleId,
                    rp.MenuId,
                    rp.ActionId,
                    m.Code AS MenuCode,
                    a.Code AS ActionCode
                FROM RolePermissions rp
                INNER JOIN Menus m ON rp.MenuId = m.Id
                INNER JOIN Actions a ON rp.ActionId = a.Id
                WHERE rp.RoleId = @RoleId
                ORDER BY m.Code, a.Code
            ";

            try
            {
                var permissions = await _dbExecutor.QueryAsync<RolePermissionDto>(
                    sql,
                    new { RoleId = roleId },
                    cancellationToken: cancellationToken);

                return permissions.Select(PermissionRepositoryMapper.ToDomain).ToList();
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve permissions for role {roleId}");
            }
        }

        public async Task<IEnumerable<ActionEntity>> GetAllActionsAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT Id, Code, Description
                FROM Actions
                ORDER BY Code
            ";

            try
            {
                var actions = await _dbExecutor.QueryAsync<ActionDto>(
                    sql,
                    cancellationToken: cancellationToken);

                return actions.Select(a => ActionEntity.Reconstitute(
                    id: a.Id,
                    code: a.Code,
                    description: a.Description
                )).ToList();
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "retrieve all actions");
            }
        }

        public async Task<IEnumerable<ActionEntity>> GetMenuActionsAsync(int menuId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT a.Id, a.Code, a.Description
                FROM MenuActions ma
                INNER JOIN Actions a ON ma.ActionId = a.Id
                WHERE ma.MenuId = @MenuId
                ORDER BY a.Code
            ";

            try
            {
                var actions = await _dbExecutor.QueryAsync<ActionDto>(
                    sql,
                    new { MenuId = menuId },
                    cancellationToken: cancellationToken);

                return actions.Select(a => ActionEntity.Reconstitute(
                    id: a.Id,
                    code: a.Code,
                    description: a.Description
                )).ToList();
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve actions for menu {menuId}");
            }
        }

        public async Task<Dictionary<int, List<int>>> GetAllMenuActionsAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT MenuId, ActionId
                FROM MenuActions
                ORDER BY MenuId, ActionId
            ";

            try
            {
                var menuActions = await _dbExecutor.QueryAsync<dynamic>(
                    sql,
                    cancellationToken: cancellationToken);

                return menuActions
                    .GroupBy(ma => (int)ma.MenuId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(ma => (int)ma.ActionId).ToList()
                    );
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "retrieve all menu actions");
            }
        }

        public async Task CreateRolePermissionAsync(int roleId, int menuId, int actionId, CancellationToken cancellationToken)
        {
            const string sql = @"
                IF NOT EXISTS (
                    SELECT 1 FROM RolePermissions 
                    WHERE RoleId = @RoleId AND MenuId = @MenuId AND ActionId = @ActionId
                )
                BEGIN
                    INSERT INTO RolePermissions (RoleId, MenuId, ActionId)
                    VALUES (@RoleId, @MenuId, @ActionId)
                END
            ";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { RoleId = roleId, MenuId = menuId, ActionId = actionId },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"create permission for role {roleId}");
            }
        }

        public async Task DeleteRolePermissionAsync(int roleId, int menuId, int actionId, CancellationToken cancellationToken)
        {
            const string sql = @"
                DELETE FROM RolePermissions
                WHERE RoleId = @RoleId AND MenuId = @MenuId AND ActionId = @ActionId
            ";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { RoleId = roleId, MenuId = menuId, ActionId = actionId },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"delete permission for role {roleId}");
            }
        }

        public async Task DeleteAllRolePermissionsAsync(int roleId, CancellationToken cancellationToken)
        {
            const string sql = @"
                DELETE FROM RolePermissions
                WHERE RoleId = @RoleId
            ";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { RoleId = roleId },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"delete all permissions for role {roleId}");
            }
        }
    }
}