using DiskyNet.Domain.Auth.Entities;
using DiskyNet.Domain.Auth.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.Auth;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Auth
{
    public class UserCredentialRepository : IUserCredentialRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public UserCredentialRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<UserCredentialEntity?> GetCredentialByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT 
                    u.UserId,
                    u.PasswordHash,
                    u.PasswordChangedAt
                FROM UserCredentials u
                WHERE u.UserId = @UserId
            """;

            var dbCredential = await _dbExecutor.QuerySingleOrDefaultAsync<UserCredentialDto>(
                sql,
                new { UserId = userId },
                commandType: null,
                cancellationToken: cancellationToken);

            if (dbCredential == null)
                return null;

            return UserCredentialEntity.Reconstitute(
                userId: dbCredential.UserId,
                passwordHash: dbCredential.PasswordHash,
                passwordChangedAt: dbCredential.PasswordChangedAt
            );
        }
    }
}

