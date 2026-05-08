using DiskyNet.Application.User.Request;
using DiskyNet.Application.User.Response;

namespace DiskyNet.Application.User.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserListResponse>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<UserResponse> GetUserByIdAsync(long id, CancellationToken cancellationToken);
        Task<long> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
        Task UpdateUserAsync(long id, UpdateUserRequest request, CancellationToken cancellationToken);
    }
}
