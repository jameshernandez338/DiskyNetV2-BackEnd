using DiskyNet.Domain.Tables.Entities;
using DiskyNet.Infrastructure.Persistence.DTOs.Tables;

namespace DiskyNet.Infrastructure.Persistence.Mappers.Tables
{
    public static class SubCategoryRepositoryMapper
    {
        public static SubCategoryEntity ToDomain(SubCategoryDto dto)
        {
            return new SubCategoryEntity
            {
                SubCategoryId = dto.SubCategory_Id,
                SubCategoryName = dto.SubCategory_Names,
                CategoryId = dto.SubCategory_CatId,
                CategoryName = dto.Category_Names
            };
        }
    }
}
