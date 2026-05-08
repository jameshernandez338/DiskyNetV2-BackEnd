using DiskyNet.Domain.Tables.Entities;
using DiskyNet.Infrastructure.Persistence.DTOs.Tables;

namespace DiskyNet.Infrastructure.Persistence.Mappers.Tables
{
    public static class TypeActivityRepositoryMapper
    {
        public static TypeActivityEntity ToDomain(TypeActivityDto dto)
        {
            return new TypeActivityEntity
            {
                TypeActivityId = dto.TypeActivity_Id,
                TypeActivityName = dto.TypeActivity_Name,
                TypeActivityFrecDays = dto.TypeActivity_FrecDays
            };
        }
    }
}
