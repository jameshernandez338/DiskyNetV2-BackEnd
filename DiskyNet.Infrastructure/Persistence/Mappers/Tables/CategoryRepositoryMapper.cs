using DiskyNet.Domain.Tables.Entities;
using DiskyNet.Infrastructure.Persistence.DTOs.Tables;

namespace DiskyNet.Infrastructure.Persistence.Mappers.Tables
{
    public static class CategoryRepositoryMapper
    {
        public static CategoryEntity ToDomain(CategoryDto dto)
        {
            return new CategoryEntity
            {
                CategoryId = dto.Category_Id,
                CategoryName = dto.Category_Names
            };
        }
    }
}
