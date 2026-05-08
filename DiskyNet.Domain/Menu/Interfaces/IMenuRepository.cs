using DiskyNet.Domain.Menu.Entities;

namespace DiskyNet.Domain.Menu.Interfaces
{
    public interface IMenuRepository
    {
        Task<IEnumerable<MenuEntity>> GetMenusByUserAsync(int userId, CancellationToken cancellationToken);
        Task<IEnumerable<MenuEntity>> GetAllMenusAsync(CancellationToken cancellationToken);
    }
}
