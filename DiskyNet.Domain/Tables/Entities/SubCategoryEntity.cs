namespace DiskyNet.Domain.Tables.Entities
{
    public class SubCategoryEntity
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
