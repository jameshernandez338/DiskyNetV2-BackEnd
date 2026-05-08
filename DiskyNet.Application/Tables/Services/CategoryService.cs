using DiskyNet.Application.Tables.Interfaces;
using DiskyNet.Application.Tables.Request;
using DiskyNet.Application.Tables.Response;
using DiskyNet.Domain.Exceptions;
using DiskyNet.Domain.Tables.Interfaces;

namespace DiskyNet.Application.Tables.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync(cancellationToken);
            return categories.Select(c => new CategoryResponse(c.CategoryId, c.CategoryName));
        }


        public async Task CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var exists = await _categoryRepository.CategoryExistsByNameAsync(request.CategoryName, null, cancellationToken);

            if (exists)
            {
                throw new ConflictException($"A category with the name '{request.CategoryName}' already exists");
            }

            await _categoryRepository.CreateCategoryAsync(request.CategoryName, cancellationToken);
        }

        public async Task UpdateCategoryAsync(int categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId, cancellationToken);

            if (category == null)
            {
                throw new DomainException($"Category with ID {categoryId} not found");
            }

            var exists = await _categoryRepository.CategoryExistsByNameAsync(request.CategoryName, categoryId, cancellationToken);

            if (exists)
            {
                throw new ConflictException($"A category with the name '{request.CategoryName}' already exists");
            }

            await _categoryRepository.UpdateCategoryAsync(categoryId, request.CategoryName, cancellationToken);
        }

        public async Task DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId, cancellationToken);

            if (category == null)
            {
                throw new DomainException($"Category with ID {categoryId} not found");
            }

            await _categoryRepository.DeleteCategoryAsync(categoryId, cancellationToken);
        }
    }
}
