using DiskyNet.Domain.Common.DataContracts;

namespace DiskyNet.Domain.Common.Interfaces
{
    public interface ICommonRepository
    {
        Task<IEnumerable<ComboItem>> GetComboDataAsync(string comboType, CancellationToken cancellationToken);
    }
}
