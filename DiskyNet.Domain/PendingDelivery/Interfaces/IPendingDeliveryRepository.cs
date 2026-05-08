using DiskyNet.Domain.PendingDelivery.Entities;

namespace DiskyNet.Domain.PendingDelivery.Interfaces
{
    public interface IPendingDeliveryRepository
    {
        Task<IEnumerable<SupplierDocumentEntity>> GetSupplierDocumentsAsync(long userId, CancellationToken cancellationToken);
    }
}
