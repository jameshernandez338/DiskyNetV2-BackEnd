using DiskyNet.Application.Auth.Reponse;
using DiskyNet.Application.Auth.Request;

namespace DiskyNet.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<LoginResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
