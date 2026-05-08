using DiskyNet.Application.Tables.Request;
using DiskyNet.Application.Tables.Response;

namespace DiskyNet.Application.Tables.Interfaces
{
    public interface ITypeActivityService
    {
        Task<IEnumerable<TypeActivityResponse>> GetAllTypeActivitiesAsync(CancellationToken cancellationToken);
        Task CreateTypeActivityAsync(CreateTypeActivityRequest request, CancellationToken cancellationToken);
        Task UpdateTypeActivityAsync(int typeActivityId, UpdateTypeActivityRequest request, CancellationToken cancellationToken);
        Task DeleteTypeActivityAsync(int typeActivityId, CancellationToken cancellationToken);
    }
}
