namespace DiskyNet.Infrastructure.Persistence.DTOs.Tables
{
    public class SubCategoryDto
    {
        public int SubCategory_Id { get; set; }
        public string SubCategory_Names { get; set; } = string.Empty;
        public int SubCategory_CatId { get; set; }
        public string Category_Names { get; set; } = string.Empty;
    }
}
