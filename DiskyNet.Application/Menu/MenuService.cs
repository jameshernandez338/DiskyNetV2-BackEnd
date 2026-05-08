using DiskyNet.Application.Menu.Interfaces;
using DiskyNet.Application.Menu.Response;
using DiskyNet.Domain.Menu.Interfaces;

namespace DiskyNet.Application.Menu
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<IEnumerable<MenuResponse>> GetMenusByUserAsync(string userId, CancellationToken cancellationToken)
        {
            var menus = await _menuRepository.GetMenusByUserAsync(userId, cancellationToken);

            return menus.Select(m => new MenuResponse(
                Id: m.Id,
                MenuName: m.MenuName,
                MenuRoute: m.MenuRoute,
                Icon: m.Icon,
                DisplayOrder: m.DisplayOrder,
                ParentId: m.ParentId,
                MenuType: m.MenuType
            ));
        }
    }
}
