using DiskyNet.Domain.Tables.Entities;

namespace DiskyNet.Domain.Tables.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync(CancellationToken cancellationToken);
        Task<CategoryEntity?> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken);
        Task<bool> CategoryExistsByNameAsync(string categoryName, int? excludeCategoryId, CancellationToken cancellationToken);
        Task<int> CreateCategoryAsync(string categoryName, CancellationToken cancellationToken);
        Task UpdateCategoryAsync(int categoryId, string categoryName, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken);
    }
}
