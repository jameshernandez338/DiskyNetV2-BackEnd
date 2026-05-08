using DiskyNet.Application.Menu.Response;

namespace DiskyNet.Application.Menu.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuResponse>> GetMenusByUserAsync(CancellationToken cancellationToken);
    }
}
