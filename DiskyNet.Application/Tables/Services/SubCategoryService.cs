using DiskyNet.Application.Tables.Interfaces;
using DiskyNet.Application.Tables.Request;
using DiskyNet.Application.Tables.Response;
using DiskyNet.Domain.Exceptions;
using DiskyNet.Domain.Tables.Interfaces;

namespace DiskyNet.Application.Tables.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SubCategoryService(
            ISubCategoryRepository subCategoryRepository,
            ICategoryRepository categoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<SubCategoryResponse>> GetAllSubCategoriesAsync(CancellationToken cancellationToken)
        {
            var subCategories = await _subCategoryRepository.GetAllSubCategoriesAsync(cancellationToken);
            return subCategories.Select(sc => new SubCategoryResponse(
                sc.SubCategoryId,
                sc.SubCategoryName,
                sc.CategoryId,
                sc.CategoryName
            ));
        }

        public async Task CreateSubCategoryAsync(CreateSubCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);

            if (category == null)
            {
                throw new DomainException($"Category with ID {request.CategoryId} not found");
            }

            var exists = await _subCategoryRepository.SubCategoryExistsByNameInCategoryAsync(
                request.SubCategoryName,
                request.CategoryId,
                null,
                cancellationToken);

            if (exists)
            {
                throw new ConflictException($"A subcategory with the name '{request.SubCategoryName}' already exists in this category");
            }

            await _subCategoryRepository.CreateSubCategoryAsync(request.SubCategoryName, request.CategoryId, cancellationToken);
        }

        public async Task UpdateSubCategoryAsync(int subCategoryId, UpdateSubCategoryRequest request, CancellationToken cancellationToken)
        {
            var subCategory = await _subCategoryRepository.GetSubCategoryByIdAsync(subCategoryId, cancellationToken);

            if (subCategory == null)
            {
                throw new DomainException($"Subcategory with ID {subCategoryId} not found");
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);

            if (category == null)
            {
                throw new DomainException($"Category with ID {request.CategoryId} not found");
            }

            var exists = await _subCategoryRepository.SubCategoryExistsByNameInCategoryAsync(
                request.SubCategoryName,
                request.CategoryId,
                subCategoryId,
                cancellationToken);

            if (exists)
            {
                throw new ConflictException($"A subcategory with the name '{request.SubCategoryName}' already exists in this category");
            }

            await _subCategoryRepository.UpdateSubCategoryAsync(subCategoryId, request.SubCategoryName, request.CategoryId, cancellationToken);
        }

        public async Task DeleteSubCategoryAsync(int subCategoryId, CancellationToken cancellationToken)
        {
            var subCategory = await _subCategoryRepository.GetSubCategoryByIdAsync(subCategoryId, cancellationToken);

            if (subCategory == null)
            {
                throw new DomainException($"Subcategory with ID {subCategoryId} not found");
            }

            await _subCategoryRepository.DeleteSubCategoryAsync(subCategoryId, cancellationToken);
        }
    }
}
