using DiskyNet.Domain.Common.DataContracts;
using DiskyNet.Domain.Common.Interfaces;
using DiskyNet.Domain.Exceptions;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.ErrorHandling;
using Microsoft.Data.SqlClient;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Common
{
    public class CommonRepository : ICommonRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public CommonRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<ComboItem>> GetComboDataAsync(string comboType, CancellationToken cancellationToken)
        {
            var sql = GetSqlByComboType(comboType);

            try
            {
                var items = await _dbExecutor.QueryAsync<ComboItem>(
                    sql,
                    parameters: null,
                    commandType: null,
                    cancellationToken: cancellationToken);

                return items;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"obtener datos del combo '{comboType}'");
            }
        }

        private static string GetSqlByComboType(string comboType)
        {
            return comboType.ToLower() switch
            {
                "roles" => @"
                    SELECT 
                        Id AS Value, 
                        Name AS Text
                    FROM Roles 
                    ORDER BY Id",

                _ => throw new DomainException($"Combo type '{comboType}' not supported")
            };
        }
    }
}

