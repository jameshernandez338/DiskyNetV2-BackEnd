using DiskyNet.Application.Tables.Interfaces;
using DiskyNet.Application.Tables.Request;
using DiskyNet.Application.Tables.Response;
using DiskyNet.Domain.Exceptions;
using DiskyNet.Domain.Tables.Interfaces;

namespace DiskyNet.Application.Tables.Services
{
    public class TypeActivityService : ITypeActivityService
    {
        private readonly ITypeActivityRepository _typeActivityRepository;

        public TypeActivityService(ITypeActivityRepository typeActivityRepository)
        {
            _typeActivityRepository = typeActivityRepository;
        }

        public async Task<IEnumerable<TypeActivityResponse>> GetAllTypeActivitiesAsync(CancellationToken cancellationToken)
        {
            var typeActivities = await _typeActivityRepository.GetAllTypeActivitiesAsync(cancellationToken);
            return typeActivities.Select(ta => new TypeActivityResponse(
                ta.TypeActivityId,
                ta.TypeActivityName,
                ta.TypeActivityFrecDays
            ));
        }

        public async Task CreateTypeActivityAsync(CreateTypeActivityRequest request, CancellationToken cancellationToken)
        {
            var exists = await _typeActivityRepository.TypeActivityExistsByNameAsync(request.TypeActivityName, null, cancellationToken);

            if (exists)
            {
                throw new ConflictException($"A type activity with the name '{request.TypeActivityName}' already exists");
            }

            await _typeActivityRepository.CreateTypeActivityAsync(request.TypeActivityName, request.TypeActivityFrecDays, cancellationToken);
        }

        public async Task UpdateTypeActivityAsync(int typeActivityId, UpdateTypeActivityRequest request, CancellationToken cancellationToken)
        {
            var typeActivity = await _typeActivityRepository.GetTypeActivityByIdAsync(typeActivityId, cancellationToken);

            if (typeActivity == null)
            {
                throw new DomainException($"Type activity with ID {typeActivityId} not found");
            }

            var exists = await _typeActivityRepository.TypeActivityExistsByNameAsync(request.TypeActivityName, typeActivityId, cancellationToken);

            if (exists)
            {
                throw new ConflictException($"A type activity with the name '{request.TypeActivityName}' already exists");
            }

            await _typeActivityRepository.UpdateTypeActivityAsync(typeActivityId, request.TypeActivityName, request.TypeActivityFrecDays, cancellationToken);
        }

        public async Task DeleteTypeActivityAsync(int typeActivityId, CancellationToken cancellationToken)
        {
            var typeActivity = await _typeActivityRepository.GetTypeActivityByIdAsync(typeActivityId, cancellationToken);

            if (typeActivity == null)
            {
                throw new DomainException($"Type activity with ID {typeActivityId} not found");
            }

            await _typeActivityRepository.DeleteTypeActivityAsync(typeActivityId, cancellationToken);
        }
    }
}
