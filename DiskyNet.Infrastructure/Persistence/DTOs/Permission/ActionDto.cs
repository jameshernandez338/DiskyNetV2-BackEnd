namespace DiskyNet.Infrastructure.Persistence.DTOs.Permission
{
    public class ActionDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
