using DiskyNet.Application.Tables.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiskyNet.Api.Controllers
{
    [ApiController]
    [Route("api/tables")]
    [Authorize]
    public class TablesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ITypeActivityService _typeActivityService;

        public TablesController(
            ICategoryService categoryService,
            ISubCategoryService subCategoryService,
            ITypeActivityService typeActivityService)
        {
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _typeActivityService = typeActivityService;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            await _categoryService.CreateCategoryAsync(request, cancellationToken);
            return Ok();
        }

        [HttpPut("categories/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            await _categoryService.UpdateCategoryAsync(categoryId, request, cancellationToken);
            return Ok();
        }

        [HttpDelete("categories/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteCategoryAsync(categoryId, cancellationToken);
            return NoContent();
        }

        [HttpGet("subcategories")]
        public async Task<IActionResult> GetAllSubCategories(CancellationToken cancellationToken)
        {
            var subCategories = await _subCategoryService.GetAllSubCategoriesAsync(cancellationToken);
            return Ok(subCategories);
        }

        [HttpPost("subcategories")]
        public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubCategoryRequest request, CancellationToken cancellationToken)
        {
            await _subCategoryService.CreateSubCategoryAsync(request, cancellationToken);
            return Ok();
        }

        [HttpPut("subcategories/{subCategoryId}")]
        public async Task<IActionResult> UpdateSubCategory(int subCategoryId, [FromBody] UpdateSubCategoryRequest request, CancellationToken cancellationToken)
        {
            await _subCategoryService.UpdateSubCategoryAsync(subCategoryId, request, cancellationToken);
            return Ok();
        }

        [HttpDelete("subcategories/{subCategoryId}")]
        public async Task<IActionResult> DeleteSubCategory(int subCategoryId, CancellationToken cancellationToken)
        {
            await _subCategoryService.DeleteSubCategoryAsync(subCategoryId, cancellationToken);
            return NoContent();
        }

        [HttpGet("type-activities")]
        public async Task<IActionResult> GetAllTypeActivities(CancellationToken cancellationToken)
        {
            var typeActivities = await _typeActivityService.GetAllTypeActivitiesAsync(cancellationToken);
            return Ok(typeActivities);
        }

        [HttpPost("type-activities")]
        public async Task<IActionResult> CreateTypeActivity([FromBody] CreateTypeActivityRequest request, CancellationToken cancellationToken)
        {
            await _typeActivityService.CreateTypeActivityAsync(request, cancellationToken);
            return Ok();
        }

        [HttpPut("type-activities/{typeActivityId}")]
        public async Task<IActionResult> UpdateTypeActivity(int typeActivityId, [FromBody] UpdateTypeActivityRequest request, CancellationToken cancellationToken)
        {
            await _typeActivityService.UpdateTypeActivityAsync(typeActivityId, request, cancellationToken);
            return Ok();
        }

        [HttpDelete("type-activities/{typeActivityId}")]
        public async Task<IActionResult> DeleteTypeActivity(int typeActivityId, CancellationToken cancellationToken)
        {
            await _typeActivityService.DeleteTypeActivityAsync(typeActivityId, cancellationToken);
            return NoContent();
        }
    }
}
