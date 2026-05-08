using DiskyNet.Domain.Auth.Entities;

namespace DiskyNet.Domain.Auth.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string Generate(AuthUserEntity user, IEnumerable<(string MenuCode, string ActionCode)>? permissions = null);
        string GenerateRefreshToken();
    }
}
