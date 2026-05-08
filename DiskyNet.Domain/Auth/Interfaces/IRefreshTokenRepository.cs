using DiskyNet.Domain.Auth.Entities;

namespace DiskyNet.Domain.Auth.Interfaces
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Obtiene un refresh token por su valor
        /// </summary>
        Task<RefreshTokenEntity?> GetByTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Guarda un nuevo refresh token
        /// </summary>
        Task SaveRefreshTokenAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken);

        /// <summary>
        /// Revoca un token específico
        /// </summary>
        Task RevokeTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Revoca todos los tokens de un usuario
        /// </summary>
        Task RevokeAllUserTokensAsync(long userId, CancellationToken cancellationToken);
    }
}
