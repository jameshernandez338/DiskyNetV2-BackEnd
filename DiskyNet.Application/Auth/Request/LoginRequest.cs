using System.ComponentModel.DataAnnotations;

namespace DiskyNet.Application.Auth.Request
{
    public record LoginRequest(
        [Required] string UserName,
        [Required] string Password
    );
}
