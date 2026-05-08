using System.Data;
using DiskyNet.Domain.PendingDelivery.Entities;
using DiskyNet.Domain.PendingDelivery.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.PendingDelivery;
using DiskyNet.Infrastructure.Persistence.ErrorHandling;
using DiskyNet.Infrastructure.Persistence.Mappers.PendingDelivery;
using Microsoft.Data.SqlClient;

namespace DiskyNet.Infrastructure.Persistence.Repositories.PendingDelivery
{
    public class PendingDeliveryRepository : IPendingDeliveryRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public PendingDeliveryRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<SupplierDocumentEntity>> GetSupplierDocumentsAsync(long userId, CancellationToken cancellationToken)
        {
            const string storedProcedure = "usp_getSupplierDocuments";

            try
            {
                var documents = await _dbExecutor.QueryAsync<SupplierDocumentDto>(
                    storedProcedure,
                    new { userId },
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken);

                return documents.Select(PendingDeliveryRepositoryMapper.ToDomain);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve supplier documents for user {userId}");
            }
        }
    }
}
