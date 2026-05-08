using DiskyNet.Application.Common.Response;

namespace DiskyNet.Application.Common.Interfaces
{
    public interface ICommonService
    {
        Task<IEnumerable<ComboItemResponse>> GetComboDataAsync(string comboType, CancellationToken cancellationToken);
    }
}
