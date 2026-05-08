namespace DiskyNet.Application.User.Response
{
    public sealed record UserResponse(
        long Id,
        string UserName,
        string Email,
        string FirstName,
        string? MiddleName,
        string LastName,
        string? SecondLastName,
        string FullName,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        int RoleId,
        string RoleName
    );
}
