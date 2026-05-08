using DiskyNet.Domain.Auth.Entities;
using DiskyNet.Domain.Auth.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.Auth;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Auth
{
    public class AuthUserRepository : IAuthUserRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public AuthUserRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<AuthUserEntity?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.IsActive,
                    ur.RoleId
                FROM Users u
                LEFT JOIN UserRole ur ON u.Id = ur.UserId
                WHERE u.UserName = @UserName
            ";

            var dbUser = await _dbExecutor.QuerySingleOrDefaultAsync<AuthUserDto>(
                sql,
                new { UserName = userName },
                commandType: null,
                cancellationToken: cancellationToken);

            if (dbUser == null)
                return null;

            return AuthUserEntity.Reconstitute(
                id: dbUser.Id,
                userName: dbUser.UserName,
                email: dbUser.Email,
                firstName: dbUser.FirstName,
                lastName: dbUser.LastName,
                isActive: dbUser.IsActive,
                roleId: dbUser.RoleId
            );
        }

        public async Task<AuthUserEntity?> GetUserByIdAsync(long userId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.IsActive,
                    ur.RoleId
                FROM Users u
                LEFT JOIN UserRole ur ON u.Id = ur.UserId
                WHERE u.Id = @UserId
            ";

            var dbUser = await _dbExecutor.QuerySingleOrDefaultAsync<AuthUserDto>(
                sql,
                new { UserId = userId },
                commandType: null,
                cancellationToken: cancellationToken);

            if (dbUser == null)
                return null;

            return AuthUserEntity.Reconstitute(
                id: dbUser.Id,
                userName: dbUser.UserName,
                email: dbUser.Email,
                firstName: dbUser.FirstName,
                lastName: dbUser.LastName,
                isActive: dbUser.IsActive,
                roleId: dbUser.RoleId
            );
        }
    }
}
