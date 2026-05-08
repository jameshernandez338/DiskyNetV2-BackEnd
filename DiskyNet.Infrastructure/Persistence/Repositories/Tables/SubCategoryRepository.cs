using DiskyNet.Domain.Tables.Entities;
using DiskyNet.Domain.Tables.Interfaces;
using DiskyNet.Infrastructure.Persistence.Dapper;
using DiskyNet.Infrastructure.Persistence.DTOs.Tables;
using DiskyNet.Infrastructure.Persistence.ErrorHandling;
using DiskyNet.Infrastructure.Persistence.Mappers.Tables;
using Microsoft.Data.SqlClient;

namespace DiskyNet.Infrastructure.Persistence.Repositories.Tables
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public SubCategoryRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<SubCategoryEntity>> GetAllSubCategoriesAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    SubCategory_Id, 
                    SubCategory_Names, 
                    SubCategory_CatId, 
                    Category_Names 
                FROM SubCategory
                LEFT JOIN Category ON Category_Id = SubCategory_CatId
                ORDER BY Category_Names, SubCategory_Names";

            try
            {
                var subCategories = await _dbExecutor.QueryAsync<SubCategoryDto>(
                    sql,
                    commandType: null,
                    cancellationToken: cancellationToken);

                return subCategories.Select(SubCategoryRepositoryMapper.ToDomain);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "retrieve subcategories");
            }
        }

        public async Task<SubCategoryEntity?> GetSubCategoryByIdAsync(int subCategoryId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    SubCategory_Id, 
                    SubCategory_Names, 
                    SubCategory_CatId, 
                    Category_Names 
                FROM SubCategory
                LEFT JOIN Category ON Category_Id = SubCategory_CatId
                WHERE SubCategory_Id = @SubCategoryId";

            try
            {
                var subCategory = await _dbExecutor.QuerySingleOrDefaultAsync<SubCategoryDto>(
                    sql,
                    new { SubCategoryId = subCategoryId },
                    cancellationToken: cancellationToken);

                return subCategory != null ? SubCategoryRepositoryMapper.ToDomain(subCategory) : null;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"retrieve subcategory with ID {subCategoryId}");
            }
        }

        public async Task<bool> SubCategoryExistsByNameInCategoryAsync(string subCategoryName, int categoryId, int? excludeSubCategoryId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1) 
                FROM SubCategory
                WHERE SubCategory_Names = @SubCategoryName
                AND SubCategory_CatId = @CategoryId
                AND (@ExcludeSubCategoryId IS NULL OR SubCategory_Id != @ExcludeSubCategoryId)";

            try
            {
                var count = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { SubCategoryName = subCategoryName, CategoryId = categoryId, ExcludeSubCategoryId = excludeSubCategoryId },
                    cancellationToken: cancellationToken);

                return count > 0;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "check if subcategory exists by name in category");
            }
        }

        public async Task<int> CreateSubCategoryAsync(string subCategoryName, int categoryId, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO SubCategory (SubCategory_Names, SubCategory_CatId)
                VALUES (@SubCategoryName, @CategoryId);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                var subCategoryId = await _dbExecutor.QuerySingleOrDefaultAsync<int>(
                    sql,
                    new { SubCategoryName = subCategoryName, CategoryId = categoryId },
                    cancellationToken: cancellationToken);

                return subCategoryId;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, "create subcategory");
            }
        }

        public async Task UpdateSubCategoryAsync(int subCategoryId, string subCategoryName, int categoryId, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE SubCategory
                SET SubCategory_Names = @SubCategoryName,
                    SubCategory_CatId = @CategoryId
                WHERE SubCategory_Id = @SubCategoryId";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { SubCategoryId = subCategoryId, SubCategoryName = subCategoryName, CategoryId = categoryId },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"update subcategory with ID {subCategoryId}");
            }
        }

        public async Task DeleteSubCategoryAsync(int subCategoryId, CancellationToken cancellationToken)
        {
            const string sql = @"
                DELETE FROM SubCategory
                WHERE SubCategory_Id = @SubCategoryId";

            try
            {
                await _dbExecutor.ExecuteAsync(
                    sql,
                    new { SubCategoryId = subCategoryId },
                    cancellationToken: cancellationToken);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionTranslator.Translate(ex, $"delete subcategory with ID {subCategoryId}");
            }
        }
    }
}
