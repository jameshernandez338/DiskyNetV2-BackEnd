using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiskyNet.Api.Controllers
{
    [ApiController]
    [Route("api/common")]
    [Authorize]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet("combos/{comboType}")]
        public async Task<IActionResult> GetComboData(string comboType, CancellationToken cancellationToken)
        {
            var data = await _commonService.GetComboDataAsync(comboType, cancellationToken);
            return Ok(data);
        }
    }
}
