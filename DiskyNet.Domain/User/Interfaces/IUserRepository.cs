using DiskyNet.Domain.User.Entities;

namespace DiskyNet.Domain.User.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<UserEntity?> GetUserByIdAsync(long id, CancellationToken cancellationToken);
        Task<UserEntity?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);
        Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<long> CreateUserAsync(UserEntity user, CancellationToken cancellationToken);
        Task UpdateUserAsync(UserEntity user, CancellationToken cancellationToken);
        Task<bool> ExistsUserNameAsync(string userName, long? excludeUserId, CancellationToken cancellationToken);
        Task<bool> ExistsEmailAsync(string email, long? excludeUserId, CancellationToken cancellationToken);
    }
}
