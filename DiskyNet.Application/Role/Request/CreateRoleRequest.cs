namespace DiskyNet.Application.Role.Request
{
    public sealed record CreateRoleRequest(
       string Name,
       string Description,
       bool IsActive = true);
}
