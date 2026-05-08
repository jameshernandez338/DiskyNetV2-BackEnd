using DiskyNet.Domain.PendingDelivery.Entities;
using DiskyNet.Infrastructure.Persistence.DTOs.PendingDelivery;

namespace DiskyNet.Infrastructure.Persistence.Mappers.PendingDelivery
{
    public static class PendingDeliveryRepositoryMapper
    {
        public static SupplierDocumentEntity ToDomain(SupplierDocumentDto dto)
        {
            return new SupplierDocumentEntity
            {
                Number = dto.Number,
                Date = dto.Date,
                Quantity = dto.Quantity,
                RemainingQuantity = dto.RemainingQuantity,
                Status = dto.Status
            };
        }
    }
}
