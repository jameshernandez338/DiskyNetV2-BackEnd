using DiskyNet.Application.Common.Interfaces;
using DiskyNet.Application.PendingDelivery.Interfaces;
using DiskyNet.Application.PendingDelivery.Response;
using DiskyNet.Domain.PendingDelivery.Interfaces;

namespace DiskyNet.Application.PendingDelivery.Services
{
    public class PendingDeliveryService : IPendingDeliveryService
    {
        private readonly IPendingDeliveryRepository _pendingDeliveryRepository;
        private readonly IUserContextService _userContextService;

        public PendingDeliveryService(
            IPendingDeliveryRepository pendingDeliveryRepository,
            IUserContextService userContextService)
        {
            _pendingDeliveryRepository = pendingDeliveryRepository;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<SupplierDocumentResponse>> GetSupplierDocumentsAsync(CancellationToken cancellationToken)
        {
            var userIdString = _userContextService.GetId();
            if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in context");
            }

            var documents = await _pendingDeliveryRepository.GetSupplierDocumentsAsync(userId, cancellationToken);

            return documents.Select(doc => new SupplierDocumentResponse(
                doc.Number,
                doc.Date,
                doc.Quantity,
                doc.RemainingQuantity,
                doc.Status
            ));
        }
    }
}
