using DiskyNet.Application.PendingDelivery.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiskyNet.Api.Controllers
{
    [ApiController]
    [Route("api/pending-delivery")]
    [Authorize]
    public class PendingDeliveryController : ControllerBase
    {
        private readonly IPendingDeliveryService _pendingDeliveryService;

        public PendingDeliveryController(IPendingDeliveryService pendingDeliveryService)
        {
            _pendingDeliveryService = pendingDeliveryService;
        }

        [HttpGet("supplier-documents")]
        public async Task<IActionResult> GetSupplierDocuments(CancellationToken cancellationToken)
        {
            var documents = await _pendingDeliveryService.GetSupplierDocumentsAsync(cancellationToken);
            return Ok(documents);
        }
    }
}
