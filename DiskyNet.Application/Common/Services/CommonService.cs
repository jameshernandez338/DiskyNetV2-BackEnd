using DiskyNet.Application.Common.Interfaces;
using DiskyNet.Application.Common.Response;
using DiskyNet.Domain.Common.Interfaces;

namespace DiskyNet.Application.Common.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;

        public CommonService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task<IEnumerable<ComboItemResponse>> GetComboDataAsync(string comboType, CancellationToken cancellationToken)
        {
            var data = await _commonRepository.GetComboDataAsync(comboType, cancellationToken);
            return data.Select(item => new ComboItemResponse(item.Value, item.Text));
        }
    }
}
