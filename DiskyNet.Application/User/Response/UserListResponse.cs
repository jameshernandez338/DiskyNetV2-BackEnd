namespace DiskyNet.Application.User.Response
{
    public sealed record UserListResponse(
        long Id,
        string UserName,
        string Email,
        string FullName,
        bool IsActive,
        string RoleName
    );
}
