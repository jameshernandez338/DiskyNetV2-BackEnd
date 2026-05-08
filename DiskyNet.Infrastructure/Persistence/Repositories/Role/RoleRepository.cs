using DiskyNet.Domain.Role.Entities;
using DiskyNet.Domain.Role.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.Role;
using DiskyNet.Infrastructure.Persistence.ErrorHandling;
using Microsoft.Data.SqlClient;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Role
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public RoleRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<RoleEntity>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    Id,
                    Name,
                    Description,
                    IsActive
                FROM Roles
                ORDER BY Name
            ";

            var dbRoles = await _dbExecutor.QueryAsync<RoleDto>(
                sql,
                commandType: null,
                cancellationToken: cancellationToken);

            return dbRoles.Select(dbRole => RoleEntity.Reconstitute(
                id: dbRole.Id,
                name: dbRole.Name,
                description: dbRole.Description,
                isActive: dbRole.IsActive
            )).ToList();
        }

        public async Task<RoleEntity?> GetRoleByIdAsync(int id, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    Id,
                    Name,
                    Description,
                    IsActive
                FROM Roles
                WHERE Id = @Id
            ";

            var dbRole = await _dbExecutor.QuerySingleOrDefaultAsync<RoleDto>(
                sql,
                new { Id = id },
                commandType: null,
                cancellationToken: cancellationToken);

            if (dbRole == null)
                return null;

            return RoleEntity.Reconstitute(
                id: dbRole.Id,
                name: dbRole.Name,
                description: dbRole.Description,
                isActive: dbRole.IsActive
            );
        }

        public async Task<int> CreateRoleAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO Roles (Name, Description, IsActive)
                VALUES (@Name, @Description, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            var parameters = new
            {
                role.Name,
                role.Description,
                role.IsActive
            };

            try
            {
                return await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    parameters,
                    commandType: null,
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "crear rol");
            }
        }

        public async Task<bool> UpdateRoleAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE Roles
                SET 
                    Name = @Name,
                    Description = @Description,
                    IsActive = @IsActive
                WHERE Id = @Id
            ";

            var parameters = new
            {
                role.Id,
                role.Name,
                role.Description,
                role.IsActive
            };

            try
            {
                var rowsAffected = await _dbExecutor.ExecuteAsync(
                    sql,
                    parameters,
                    commandType: null,
                    cancellationToken: cancellationToken);

                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "actualizar rol");
            }
        }

        public async Task<bool> ExistsNameAsync(string name, int? excludeId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Roles
                WHERE Name = @Name
                  AND (@ExcludeId IS NULL OR Id != @ExcludeId)
            ";

            try
            {
                var count = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { Name = name, ExcludeId = excludeId },
                    cancellationToken: cancellationToken);

                return count > 0;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "verificar existencia del nombre del rol");
            }
        }
    }
}