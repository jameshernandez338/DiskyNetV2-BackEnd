namespace DiskyNet.Application.PendingDelivery.Response
{
    public record SupplierDocumentResponse(
        string Number,
        string Date,
        decimal Quantity,
        decimal RemainingQuantity,
        string Status
    );
}
