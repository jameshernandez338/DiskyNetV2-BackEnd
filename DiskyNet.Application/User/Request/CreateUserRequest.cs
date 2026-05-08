using System.ComponentModel.DataAnnotations;

namespace DiskyNet.Application.User.Request
{
    public sealed record CreateUserRequest(
        [Required][MaxLength(50)] string UserName,
        [Required][EmailAddress][MaxLength(100)] string Email,
        [Required][MaxLength(50)] string FirstName,
        [MaxLength(50)] string? MiddleName,
        [Required][MaxLength(50)] string LastName,
        [MaxLength(50)] string? SecondLastName,
        [Required][Range(1, int.MaxValue)] int RoleId,
        bool IsActive = true
    );
}
