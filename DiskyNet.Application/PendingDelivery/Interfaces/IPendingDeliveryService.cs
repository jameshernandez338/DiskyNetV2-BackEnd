using DiskyNet.Application.PendingDelivery.Response;

namespace DiskyNet.Application.PendingDelivery.Interfaces
{
    public interface IPendingDeliveryService
    {
        Task<IEnumerable<SupplierDocumentResponse>> GetSupplierDocumentsAsync(CancellationToken cancellationToken);
    }
}
