namespace DiskyNet.Domain.PendingDelivery.Entities
{
    public class SupplierDocumentEntity
    {
        public string Number { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal RemainingQuantity { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
