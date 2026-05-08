using DiskyNet.Application.Tables.Request;
using DiskyNet.Application.Tables.Response;

namespace DiskyNet.Application.Tables.Interfaces
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryResponse>> GetAllSubCategoriesAsync(CancellationToken cancellationToken);
        Task CreateSubCategoryAsync(CreateSubCategoryRequest request, CancellationToken cancellationToken);
        Task UpdateSubCategoryAsync(int subCategoryId, UpdateSubCategoryRequest request, CancellationToken cancellationToken);
        Task DeleteSubCategoryAsync(int subCategoryId, CancellationToken cancellationToken);
    }
}
