namespace DiskyNet.Infrastructure.Persistence.DTOs.Menu
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? MenuRoute { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }
        public int? ParentId { get; set; }
        public string MenuType { get; set; } = string.Empty;
    }
}
