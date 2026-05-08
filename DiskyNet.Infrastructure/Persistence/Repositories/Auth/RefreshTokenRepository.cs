using DiskyNet.Domain.Auth.Entities;
using DiskyNet.Domain.Auth.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.Auth;
using System.Data;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Auth
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public RefreshTokenRepository(IDbExecutor dbExecutor) => _dbExecutor = dbExecutor;

        public async Task<RefreshTokenEntity?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            const string spName = "usp_getRefreshToken";

            var dbToken = await _dbExecutor.QuerySingleOrDefaultAsync<RefreshTokenDto>(
                spName,
                new { token = token },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            if (dbToken == null)
                return null;

            return RefreshTokenEntity.Reconstitute(
                token: dbToken.Token,
                userId: dbToken.UserId,
                expiresAt: dbToken.ExpiresAt,
                createdAt: dbToken.CreatedAt,
                isRevoked: dbToken.IsRevoked
            );
        }

        public async Task SaveRefreshTokenAsync(
            RefreshTokenEntity refreshToken,
            CancellationToken cancellationToken)
        {
            const string spName = "usp_saveRefreshToken";

            await _dbExecutor.ExecuteAsync(
                spName,
                new
                {
                    token = refreshToken.Token,
                    userId = refreshToken.UserId,
                    expiresAt = refreshToken.ExpiresAt
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);
        }

        public async Task RevokeTokenAsync(string token, CancellationToken cancellationToken)
        {
            const string spName = "usp_revokeRefreshToken";

            await _dbExecutor.ExecuteAsync(
                spName,
                new { token = token },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);
        }

        public async Task RevokeAllUserTokensAsync(long userId, CancellationToken cancellationToken)
        {
            const string spName = "usp_revokeAllUserRefreshTokens";

            await _dbExecutor.ExecuteAsync(
                spName,
                new { userId = userId },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);
        }
    }
}

