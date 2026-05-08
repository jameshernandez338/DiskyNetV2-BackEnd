namespace DiskyNet.Application.Role.Request
{
    public sealed record UpdateRoleRequest(
        int Id,
        string Name,
        string Description,
        bool IsActive);
}
