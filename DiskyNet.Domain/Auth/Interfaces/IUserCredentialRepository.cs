using DiskyNet.Domain.Auth.Entities;

namespace DiskyNet.Domain.Auth.Interfaces
{
    public interface IUserCredentialRepository
    {
        Task<UserCredentialEntity?> GetCredentialByUserIdAsync(long userId, CancellationToken cancellationToken);
    }
}
