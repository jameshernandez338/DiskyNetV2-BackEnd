using DiskyNet.Application.Common.Interfaces;
using DiskyNet.Application.Menu.Interfaces;
using DiskyNet.Application.Menu.Response;
using DiskyNet.Domain.Menu.Interfaces;

namespace DiskyNet.Application.Menu
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUserContextService _userContextService;

        public MenuService(
            IMenuRepository menuRepository,
            IUserContextService userContextService)
        {
            _menuRepository = menuRepository;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<MenuResponse>> GetMenusByUserAsync(CancellationToken cancellationToken)
        {
            var userIdString = _userContextService.GetId();
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in context");
            }

            var menus = await _menuRepository.GetMenusByUserAsync(userId, cancellationToken);

            return menus.Select(m => new MenuResponse(
                Id: m.Id,
                MenuName: m.MenuName,
                Code: m.Code,
                MenuRoute: m.MenuRoute,
                Icon: m.Icon,
                DisplayOrder: m.DisplayOrder,
                ParentId: m.ParentId,
                MenuType: m.MenuType
            ));
        }
    }
}
