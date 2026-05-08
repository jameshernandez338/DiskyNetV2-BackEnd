using DiskyNet.Application.User.Interfaces;
using DiskyNet.Application.User.Request;
using DiskyNet.Application.User.Response;
using DiskyNet.Domain.Exceptions;
using DiskyNet.Domain.Role.Interfaces;
using DiskyNet.Domain.User.Entities;
using DiskyNet.Domain.User.Interfaces;

namespace DiskyNet.Application.User.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<UserListResponse>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsersAsync(cancellationToken);
            var allRoles = await _roleRepository.GetAllRolesAsync(cancellationToken);
            var roleDict = allRoles.ToDictionary(r => r.Id, r => r.Name);

            return users.Select(user => new UserListResponse(
                Id: user.Id,
                UserName: user.UserName,
                Email: user.Email,
                FullName: user.GetFullName(),
                IsActive: user.IsActive,
                RoleName: roleDict.GetValueOrDefault(user.RoleId, "Unknown")
            ));
        }

        public async Task<UserResponse> GetUserByIdAsync(long id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
                throw new DomainException($"User with ID {id} not found");

            var allRoles = await _roleRepository.GetAllRolesAsync(cancellationToken);
            var roleDict = allRoles.ToDictionary(r => r.Id, r => r.Name);

            return new UserResponse(
                Id: user.Id,
                UserName: user.UserName,
                Email: user.Email,
                FirstName: user.FirstName,
                MiddleName: user.MiddleName,
                LastName: user.LastName,
                SecondLastName: user.SecondLastName,
                FullName: user.GetFullName(),
                IsActive: user.IsActive,
                CreatedAt: user.CreatedAt,
                UpdatedAt: user.UpdatedAt,
                RoleId: user.RoleId,
                RoleName: roleDict.GetValueOrDefault(user.RoleId, "Unknown")
            );
        }

        public async Task<long> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            // Validate role exists
            await ValidateRoleExistsAsync(request.RoleId, cancellationToken);

            // Check username uniqueness
            if (await _userRepository.ExistsUserNameAsync(request.UserName, null, cancellationToken))
                throw new ConflictException($"Username '{request.UserName}' already exists");

            // Check email uniqueness
            if (await _userRepository.ExistsEmailAsync(request.Email, null, cancellationToken))
                throw new ConflictException($"Email '{request.Email}' already exists");

            var user = UserEntity.Create(
                userName: request.UserName,
                email: request.Email,
                firstName: request.FirstName,
                middleName: request.MiddleName,
                lastName: request.LastName,
                secondLastName: request.SecondLastName,
                roleId: request.RoleId,
                isActive: request.IsActive
            );

            return await _userRepository.CreateUserAsync(user, cancellationToken);
        }

        public async Task UpdateUserAsync(long id, UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
                throw new DomainException($"User with ID {id} not found");

            // Validate role exists
            await ValidateRoleExistsAsync(request.RoleId, cancellationToken);

            // Check username uniqueness
            if (await _userRepository.ExistsUserNameAsync(request.UserName, id, cancellationToken))
                throw new ConflictException($"Username '{request.UserName}' already exists");

            // Check email uniqueness
            if (await _userRepository.ExistsEmailAsync(request.Email, id, cancellationToken))
                throw new ConflictException($"Email '{request.Email}' already exists");

            user.Update(
                userName: request.UserName,
                email: request.Email,
                firstName: request.FirstName,
                middleName: request.MiddleName,
                lastName: request.LastName,
                secondLastName: request.SecondLastName,
                roleId: request.RoleId
            );

            if (request.IsActive && !user.IsActive)
                user.Activate();
            else if (!request.IsActive && user.IsActive)
                user.Deactivate();

            await _userRepository.UpdateUserAsync(user, cancellationToken);
        }

        private async Task ValidateRoleExistsAsync(int roleId, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId, cancellationToken);

            if (role == null)
                throw new DomainException($"Role with ID {roleId} does not exist");
        }
    }
}
