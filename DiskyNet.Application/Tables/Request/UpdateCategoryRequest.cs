using System.ComponentModel.DataAnnotations;

namespace DiskyNet.Application.Tables.Request
{
    public class UpdateCategoryRequest
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
