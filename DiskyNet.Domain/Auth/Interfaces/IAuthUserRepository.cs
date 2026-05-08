using DiskyNet.Domain.Auth.Entities;

namespace DiskyNet.Domain.Auth.Interfaces
{
    public interface IAuthUserRepository
    {
        /// <summary>
        /// Obtiene un usuario por su nombre de usuario (email)
        /// </summary>
        Task<AuthUserEntity?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        Task<AuthUserEntity?> GetUserByIdAsync(long userId, CancellationToken cancellationToken);
    }
}
