namespace DiskyNet.Application.Role.Response
{
    public sealed record RoleResponse(
        int Id,
        string Name,
        string Description,
        bool IsActive);
}
