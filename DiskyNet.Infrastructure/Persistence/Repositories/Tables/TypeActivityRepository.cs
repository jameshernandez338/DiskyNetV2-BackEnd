using DiskyNet.Domain.Tables.Entities;
using DiskyNet.Domain.Tables.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.Tables;
using DiskyNet.Infrastructure.Persistence.ErrorHandling;
using DiskyNet.Infrastructure.Persistence.Mappers.Tables;
using Microsoft.Data.SqlClient;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Tables
{
    public class TypeActivityRepository : ITypeActivityRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public TypeActivityRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<TypeActivityEntity>> GetAllTypeActivitiesAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    TypeActivity_Id, 
                    TypeActivity_Name, 
                    TypeActivity_FrecDays 
                FROM TypeActivity
                ORDER BY TypeActivity_Name";

            try
            {
                var typeActivities = await _dbExecutor.QueryAsync<TypeActivityDto>(
                    sql,
                    commandType: null,
                    cancellationToken: cancellationToken);

                return typeActivities.Select(TypeActivityRepositoryMapper.ToDomain);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "retrieve type activities");
            }
        }

        public async Task<TypeActivityEntity?> GetTypeActivityByIdAsync(int typeActivityId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    TypeActivity_Id, 
                    TypeActivity_Name, 
                    TypeActivity_FrecDays 
                FROM TypeActivity
                WHERE TypeActivity_Id = @TypeActivityId";

            try
            {
                var typeActivity = await _dbExecutor.QuerySingleOrDefaultAsync<TypeActivityDto>(
                    sql,
                    new { TypeActivityId = typeActivityId },
                    cancellationToken: cancellationToken);

                return typeActivity != null ? TypeActivityRepositoryMapper.ToDomain(typeActivity) : null;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve type activity with ID {typeActivityId}");
            }
        }

        public async Task<bool> TypeActivityExistsByNameAsync(string typeActivityName, int? excludeTypeActivityId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1) 
                FROM TypeActivity
                WHERE TypeActivity_Name = @TypeActivityName
                AND (@ExcludeTypeActivityId IS NULL OR TypeActivity_Id != @ExcludeTypeActivityId)";

            try
            {
                var count = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { TypeActivityName = typeActivityName, ExcludeTypeActivityId = excludeTypeActivityId },
                    cancellationToken: cancellationToken);

                return count > 0;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "check if type activity exists by name");
            }
        }

        public async Task<int> CreateTypeActivityAsync(string typeActivityName, int typeActivityFrecDays, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO TypeActivity (TypeActivity_Name, TypeActivity_FrecDays)
                VALUES (@TypeActivityName, @TypeActivityFrecDays);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                var typeActivityId = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { TypeActivityName = typeActivityName, TypeActivityFrecDays = typeActivityFrecDays },
                    cancellationToken: cancellationToken);

                return typeActivityId;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "create type activity");
            }
        }

        public async Task UpdateTypeActivityAsync(int typeActivityId, string typeActivityName, int typeActivityFrecDays, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE TypeActivity
                SET TypeActivity_Name = @TypeActivityName,
                    TypeActivity_FrecDays = @TypeActivityFrecDays
                WHERE TypeActivity_Id = @TypeActivityId";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { TypeActivityId = typeActivityId, TypeActivityName = typeActivityName, TypeActivityFrecDays = typeActivityFrecDays },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"update type activity with ID {typeActivityId}");
            }
        }

        public async Task DeleteTypeActivityAsync(int typeActivityId, CancellationToken cancellationToken)
        {
            const string sql = @"
                DELETE FROM TypeActivity
                WHERE TypeActivity_Id = @TypeActivityId";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { TypeActivityId = typeActivityId },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"delete type activity with ID {typeActivityId}");
            }
        }
    }
}
