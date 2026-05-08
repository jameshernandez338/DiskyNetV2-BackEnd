using DiskyNet.Domain.User.Entities;
using DiskyNet.Domain.User.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.User;
using DiskyNet.Infrastructure.Persistence.ErrorHandling;
using DiskyNet.Infrastructure.Persistence.Mappers.User;
using Microsoft.Data.SqlClient;

namespace DiskyNet.Infrastructure.Persistence.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public UserRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.MiddleName,
                    u.LastName,
                    u.SecondLastName,
                    u.IsActive,
                    u.CreatedAt,
                    u.UpdatedAt
                FROM Users u
                ORDER BY u.FirstName, u.LastName
            ";

            try
            {
                var users = await _dbExecutor.QueryAsync<UserDto>(
                    sql,
                    commandType: null,
                    cancellationToken: cancellationToken);

                var userList = users.ToList();
                if (!userList.Any()) return new List<UserEntity>();

                // Get all user roles
                var userIds = userList.Select(u => u.Id).ToList();
                var userRoles = await GetUserRolesAsync(userIds, cancellationToken);
                var userRoleDict = userRoles.ToDictionary(ur => ur.UserId, ur => ur.RoleId);

                return userList.Select(user =>
                {
                    var roleId = userRoleDict.GetValueOrDefault(user.Id, 0);
                    return UserRepositoryMapper.ToDomain(user, roleId);
                }).ToList();
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "retrieve users");
            }
        }

        public async Task<UserEntity?> GetUserByIdAsync(long id, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.MiddleName,
                    u.LastName,
                    u.SecondLastName,
                    u.IsActive,
                    u.CreatedAt,
                    u.UpdatedAt
                FROM Users u
                WHERE u.Id = @Id
            ";

            try
            {
                var user = await _dbExecutor.QuerySingleOrDefaultAsync<UserDto>(
                    sql,
                    new { Id = id },
                    cancellationToken: cancellationToken);

                if (user == null) return null;

                var roleId = await GetUserRoleIdAsync(id, cancellationToken);

                return UserRepositoryMapper.ToDomain(user, roleId);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve user with ID {id}");
            }
        }

        public async Task<UserEntity?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.MiddleName,
                    u.LastName,
                    u.SecondLastName,
                    u.IsActive,
                    u.CreatedAt,
                    u.UpdatedAt
                FROM Users u
                WHERE u.UserName = @UserName
            ";

            try
            {
                var user = await _dbExecutor.QuerySingleOrDefaultAsync<UserDto>(
                    sql,
                    new { UserName = userName },
                    cancellationToken: cancellationToken);

                if (user == null) return null;

                var roleId = await GetUserRoleIdAsync(user.Id, cancellationToken);

                return UserRepositoryMapper.ToDomain(user, roleId);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve user by username '{userName}'");
            }
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.MiddleName,
                    u.LastName,
                    u.SecondLastName,
                    u.IsActive,
                    u.CreatedAt,
                    u.UpdatedAt
                FROM Users u
                WHERE u.Email = @Email
            ";

            try
            {
                var user = await _dbExecutor.QuerySingleOrDefaultAsync<UserDto>(
                    sql,
                    new { Email = email },
                    cancellationToken: cancellationToken);

                if (user == null) return null;

                var roleId = await GetUserRoleIdAsync(user.Id, cancellationToken);

                return UserRepositoryMapper.ToDomain(user, roleId);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve user by email '{email}'");
            }
        }

        public async Task<long> CreateUserAsync(UserEntity user, CancellationToken cancellationToken)
        {
            const string sqlInsertUser = @"
                INSERT INTO Users (
                    UserName,
                    Email,
                    FirstName,
                    MiddleName,
                    LastName,
                    SecondLastName,
                    IsActive,
                    CreatedAt
                )
                VALUES (
                    @UserName,
                    @Email,
                    @FirstName,
                    @MiddleName,
                    @LastName,
                    @SecondLastName,
                    @IsActive,
                    GETDATE()
                );
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);
            ";

            try
            {
                var userId = await _dbExecutor.QuerySingleOrDefaultAsync<long>(
                    sqlInsertUser,
                    new
                    {
                        user.UserName,
                        user.Email,
                        user.FirstName,
                        user.MiddleName,
                        user.LastName,
                        user.SecondLastName,
                        user.IsActive,
                        user.CompanyCode
                    },
                    cancellationToken: cancellationToken);

                // Insert user role
                await InsertUserRoleAsync(userId, user.RoleId, cancellationToken);

                return userId;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "create user");
            }
        }

        public async Task UpdateUserAsync(UserEntity user, CancellationToken cancellationToken)
        {
            const string sqlUpdateUser = @"
                UPDATE Users
                SET 
                    UserName = @UserName,
                    Email = @Email,
                    FirstName = @FirstName,
                    MiddleName = @MiddleName,
                    LastName = @LastName,
                    SecondLastName = @SecondLastName,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE()
                WHERE Id = @Id
            ";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sqlUpdateUser,
                    new
                    {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.FirstName,
                        user.MiddleName,
                        user.LastName,
                        user.SecondLastName,
                        user.IsActive
                    },
                    cancellationToken: cancellationToken);

                // Update user role
                await UpdateUserRoleAsync(user.Id, user.RoleId, cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "update user");
            }
        }

        public async Task<bool> ExistsUserNameAsync(string userName, long? excludeUserId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE UserName = @UserName
                  AND (@ExcludeUserId IS NULL OR Id != @ExcludeUserId)
            ";

            try
            {
                var count = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { UserName = userName, ExcludeUserId = excludeUserId },
                    cancellationToken: cancellationToken);

                return count > 0;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "check username existence");
            }
        }

        public async Task<bool> ExistsEmailAsync(string email, long? excludeUserId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE Email = @Email
                  AND (@ExcludeUserId IS NULL OR Id != @ExcludeUserId)
            ";

            try
            {
                var count = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { Email = email, ExcludeUserId = excludeUserId },
                    cancellationToken: cancellationToken);

                return count > 0;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "check email existence");
            }
        }

        // Private helper methods
        private async Task<int> GetUserRoleIdAsync(long userId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT TOP 1 RoleId
                FROM UserRole
                WHERE UserId = @UserId
            ";

            try
            {
                var roleId = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { UserId = userId },
                    cancellationToken: cancellationToken);

                return roleId;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve role for user {userId}");
            }
        }

        private async Task<List<UserRoleDto>> GetUserRolesAsync(List<long> userIds, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT UserId, RoleId
                FROM UserRole
                WHERE UserId IN @UserIds
            ";

            try
            {
                var userRoles = await _dbExecutor.QueryAsync<UserRoleDto>(
                    sql,
                    new { UserIds = userIds },
                    cancellationToken: cancellationToken);

                return userRoles.ToList();
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "retrieve user roles");
            }
        }

        private async Task InsertUserRoleAsync(long userId, int roleId, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO UserRole (UserId, RoleId)
                VALUES (@UserId, @RoleId)
            ";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { UserId = userId, RoleId = roleId },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "insert user role");
            }
        }

        private async Task UpdateUserRoleAsync(long userId, int roleId, CancellationToken cancellationToken)
        {
            // Delete existing role
            const string sqlDelete = @"
                DELETE FROM UserRole
                WHERE UserId = @UserId
            ";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sqlDelete,
                    new { UserId = userId },
                    cancellationToken: cancellationToken);

                // Insert new role
                await InsertUserRoleAsync(userId, roleId, cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "update user role");
            }
        }
    }
}
