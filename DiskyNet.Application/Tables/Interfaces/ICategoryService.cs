using DiskyNet.Application.Tables.Request;
using DiskyNet.Application.Tables.Response;

namespace DiskyNet.Application.Tables.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(CancellationToken cancellationToken);
        Task CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
        Task UpdateCategoryAsync(int categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken);
    }
}
