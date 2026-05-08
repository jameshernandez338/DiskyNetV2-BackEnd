using DiskyNet.Domain.Tables.Entities;
using DiskyNet.Domain.Tables.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.Tables;
using DiskyNet.Infrastructure.Persistence.ErrorHandling;
using DiskyNet.Infrastructure.Persistence.Mappers.Tables;
using Microsoft.Data.SqlClient;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Tables
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public CategoryRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    Category_Id, 
                    Category_Names 
                FROM Category
                ORDER BY Category_Names";

            try
            {
                var categories = await _dbExecutor.QueryAsync<CategoryDto>(
                    sql,
                    commandType: null,
                    cancellationToken: cancellationToken);

                return categories.Select(CategoryRepositoryMapper.ToDomain);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "retrieve categories");
            }
        }

        public async Task<CategoryEntity?> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    Category_Id, 
                    Category_Names 
                FROM Category
                WHERE Category_Id = @CategoryId";

            try
            {
                var category = await _dbExecutor.QuerySingleOrDefaultAsync<CategoryDto>(
                    sql,
                    new { CategoryId = categoryId },
                    cancellationToken: cancellationToken);

                return category != null ? CategoryRepositoryMapper.ToDomain(category) : null;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve category with ID {categoryId}");
            }
        }

        public async Task<bool> CategoryExistsByNameAsync(string categoryName, int? excludeCategoryId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1) 
                FROM Category
                WHERE Category_Names = @CategoryName
                AND (@ExcludeCategoryId IS NULL OR Category_Id != @ExcludeCategoryId)";

            try
            {
                var count = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { CategoryName = categoryName, ExcludeCategoryId = excludeCategoryId },
                    cancellationToken: cancellationToken);

                return count > 0;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "check if category exists by name");
            }
        }

        public async Task<int> CreateCategoryAsync(string categoryName, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO Category (Category_Names)
                VALUES (@CategoryName);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                var categoryId = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { CategoryName = categoryName },
                    cancellationToken: cancellationToken);

                return categoryId;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "create category");
            }
        }

        public async Task UpdateCategoryAsync(int categoryId, string categoryName, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE Category
                SET Category_Names = @CategoryName
                WHERE Category_Id = @CategoryId";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { CategoryId = categoryId, CategoryName = categoryName },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"update category with ID {categoryId}");
            }
        }

        public async Task DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken)
        {
            const string sql = @"
                DELETE FROM Category
                WHERE Category_Id = @CategoryId";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { CategoryId = categoryId },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"delete category with ID {categoryId}");
            }
        }
    }
}
