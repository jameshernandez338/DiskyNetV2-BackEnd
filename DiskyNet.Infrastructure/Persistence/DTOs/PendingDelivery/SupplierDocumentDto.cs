namespace DiskyNet.Infrastructure.Persistence.DTOs.PendingDelivery
{
    public class SupplierDocumentDto
    {
        public string Number { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal RemainingQuantity { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
