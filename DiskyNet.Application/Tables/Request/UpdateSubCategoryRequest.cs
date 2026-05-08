using System.ComponentModel.DataAnnotations;

namespace DiskyNet.Application.Tables.Request
{
    public class UpdateSubCategoryRequest
    {
        [Required(ErrorMessage = "Subcategory name is required")]
        [StringLength(100, ErrorMessage = "Subcategory name cannot exceed 100 characters")]
        public string SubCategoryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
        public int CategoryId { get; set; }
    }
}
