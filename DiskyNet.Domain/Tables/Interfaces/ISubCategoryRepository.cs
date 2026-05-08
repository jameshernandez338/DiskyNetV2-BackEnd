using DiskyNet.Domain.Tables.Entities;

namespace DiskyNet.Domain.Tables.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<IEnumerable<SubCategoryEntity>> GetAllSubCategoriesAsync(CancellationToken cancellationToken);
        Task<SubCategoryEntity?> GetSubCategoryByIdAsync(int subCategoryId, CancellationToken cancellationToken);
        Task<bool> SubCategoryExistsByNameInCategoryAsync(string subCategoryName, int categoryId, int? excludeSubCategoryId, CancellationToken cancellationToken);
        Task<int> CreateSubCategoryAsync(string subCategoryName, int categoryId, CancellationToken cancellationToken);
        Task UpdateSubCategoryAsync(int subCategoryId, string subCategoryName, int categoryId, CancellationToken cancellationToken);
        Task DeleteSubCategoryAsync(int subCategoryId, CancellationToken cancellationToken);
    }
}
