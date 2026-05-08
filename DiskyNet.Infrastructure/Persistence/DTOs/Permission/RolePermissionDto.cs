namespace DiskyNet.Infrastructure.Persistence.DTOs.Permission
{
    public class RolePermissionDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public int ActionId { get; set; }
        public string MenuCode { get; set; } = string.Empty;
        public string ActionCode { get; set; } = string.Empty;
    }
}
