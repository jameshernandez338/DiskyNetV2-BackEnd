using DiskyNet.Application.Role.Interfaces;
using DiskyNet.Application.Role.Request;
using DiskyNet.Application.Role.Response;
using DiskyNet.Domain.Exceptions;
using DiskyNet.Domain.Role.Entities;
using DiskyNet.Domain.Role.Interfaces;

namespace DiskyNet.Application.Role.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllRolesAsync(cancellationToken);

            return roles.Select(r => new RoleResponse(
                Id: r.Id,
                Name: r.Name,
                Description: r.Description,
                IsActive: r.IsActive
            ));
        }

        public async Task<RoleResponse?> GetRoleByIdAsync(int id, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id, cancellationToken);

            if (role == null)
                return null;

            return new RoleResponse(
                Id: role.Id,
                Name: role.Name,
                Description: role.Description,
                IsActive: role.IsActive
            );
        }

        public async Task<int> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken)
        {
            // Validate name uniqueness
            var exists = await _roleRepository.ExistsNameAsync(request.Name, null, cancellationToken);
            if (exists)
            {
                throw new ConflictException($"Ya existe un rol con el nombre '{request.Name}'.");
            }

            var role = RoleEntity.Create(
                name: request.Name,
                description: request.Description,
                isActive: request.IsActive
            );

            return await _roleRepository.CreateRoleAsync(role, cancellationToken);
        }

        public async Task<bool> UpdateRoleAsync(int id, UpdateRoleRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id, cancellationToken);

            if (role == null)
                return false;

            // Validate name uniqueness (excluding current role)
            var exists = await _roleRepository.ExistsNameAsync(request.Name, id, cancellationToken);
            if (exists)
            {
                throw new ConflictException($"Ya existe un rol con el nombre '{request.Name}'.");
            }

            role.Update(request.Name, request.Description, request.IsActive);

            return await _roleRepository.UpdateRoleAsync(role, cancellationToken);
        }
    }
}
