using DiskyNet.Domain.Menu.Entities;
using DiskyNet.Domain.Menu.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.Menu;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Menu
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public MenuRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<MenuEntity>> GetMenusByUserAsync(string userId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    m.Id,
                    m.MenuName,
                    m.Code,
                    m.MenuRoute,
                    m.Icon,
                    m.DisplayOrder,
                    m.ParentId,
                    m.MenuType
                FROM Menus m
                WHERE m.IsActive = 1
                ORDER BY m.DisplayOrder
            ";

            var dbMenus = await _dbExecutor.QueryAsync<MenuDto>(
                sql,
                new { UserId = userId },
                commandType: null,
                cancellationToken: cancellationToken);

            return dbMenus.Select(dbMenu => MenuEntity.Reconstitute(
                id: dbMenu.Id,
                menuName: dbMenu.MenuName,
                code: dbMenu.Code,
                menuRoute: dbMenu.MenuRoute,
                icon: dbMenu.Icon,
                displayOrder: dbMenu.DisplayOrder,
                parentId: dbMenu.ParentId,
                menuType: dbMenu.MenuType
            )).ToList();
        }

        public async Task<IEnumerable<MenuEntity>> GetAllMenusAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    m.Id,
                    m.MenuName,
                    m.Code,
                    m.MenuRoute,
                    m.Icon,
                    m.DisplayOrder,
                    m.ParentId,
                    m.MenuType
                FROM Menus m
                WHERE m.IsActive = 1
                ORDER BY m.DisplayOrder
            ";

            var dbMenus = await _dbExecutor.QueryAsync<MenuDto>(
                sql,
                commandType: null,
                cancellationToken: cancellationToken);

            return dbMenus.Select(dbMenu => MenuEntity.Reconstitute(
                id: dbMenu.Id,
                menuName: dbMenu.MenuName,
                code: dbMenu.Code,
                menuRoute: dbMenu.MenuRoute,
                icon: dbMenu.Icon,
                displayOrder: dbMenu.DisplayOrder,
                parentId: dbMenu.ParentId,
                menuType: dbMenu.MenuType
            )).ToList();
        }
    }
}
