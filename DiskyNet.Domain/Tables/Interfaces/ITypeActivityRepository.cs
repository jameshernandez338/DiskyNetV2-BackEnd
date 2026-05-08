using DiskyNet.Domain.Tables.Entities;

namespace DiskyNet.Domain.Tables.Interfaces
{
    public interface ITypeActivityRepository
    {
        Task<IEnumerable<TypeActivityEntity>> GetAllTypeActivitiesAsync(CancellationToken cancellationToken);
        Task<TypeActivityEntity?> GetTypeActivityByIdAsync(int typeActivityId, CancellationToken cancellationToken);
        Task<bool> TypeActivityExistsByNameAsync(string typeActivityName, int? excludeTypeActivityId, CancellationToken cancellationToken);
        Task<int> CreateTypeActivityAsync(string typeActivityName, int typeActivityFrecDays, CancellationToken cancellationToken);
        Task UpdateTypeActivityAsync(int typeActivityId, string typeActivityName, int typeActivityFrecDays, CancellationToken cancellationToken);
        Task DeleteTypeActivityAsync(int typeActivityId, CancellationToken cancellationToken);
    }
}
