using DiskyNet.Application.Menu.Response;

namespace DiskyNet.Application.Menu.Interfaces
{
    public interface IMenuService
    {
        /// <summary>
        /// Obtiene los menús disponibles para un usuario
        /// </summary>
        Task<IEnumerable<MenuResponse>> GetMenusByUserAsync(string userId, CancellationToken cancellationToken);
    }
}
