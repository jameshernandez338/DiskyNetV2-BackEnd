using DiskyNet.Application.Auth.Interfaces;
using DiskyNet.Application.Auth.Reponse;
using DiskyNet.Application.Auth.Request;
using DiskyNet.Application.Auth.Response;
using DiskyNet.Domain.Auth.Entities;
using DiskyNet.Domain.Auth.Interfaces;
using DiskyNet.Domain.Exceptions;
using DiskyNet.Domain.Permission.Interfaces;

namespace DiskyNet.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthUserRepository _userRepository;
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPermissionRepository _permissionRepository;

        public AuthService(
            IAuthUserRepository userRepository,
            IUserCredentialRepository userCredentialRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IPermissionRepository permissionRepository)
        {
            _userRepository = userRepository;
            _userCredentialRepository = userCredentialRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _permissionRepository = permissionRepository;
        }

        public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Obtener usuario
                var user = await _userRepository.GetUserByUserNameAsync(request.UserName, cancellationToken);
                if (user is null)
                    return LoginResult.Fail("Credenciales inválidas");

                // Validar que puede hacer login (regla de negocio en el dominio)
                user.ValidateCanLogin();

                // Verificar credenciales
                var credentials = await _userCredentialRepository.GetCredentialByUserIdAsync(user.Id, cancellationToken);
                if (credentials is null)
                    return LoginResult.Fail("Credenciales inválidas");

                if (!_passwordHasher.Verify(request.Password, credentials.PasswordHash))
                    return LoginResult.Fail("Credenciales inválidas");

                // Obtener permisos del rol del usuario
                var permissions = new List<UserPermissionResponse>();
                if (user.RoleId.HasValue)
                {
                    var rolePermissions = await _permissionRepository.GetRolePermissionsAsync(user.RoleId.Value, cancellationToken);
                    permissions = rolePermissions.Select(p => new UserPermissionResponse(
                        MenuCode: p.MenuCode ?? string.Empty,
                        ActionCode: p.ActionCode ?? string.Empty
                    )).ToList();
                }

                // Generar tokens con permisos incluidos en el JWT
                var permissionTuples = permissions.Select(p => (p.MenuCode, p.ActionCode));
                var accessToken = _jwtTokenGenerator.Generate(user, permissionTuples);
                var refreshTokenValue = _jwtTokenGenerator.GenerateRefreshToken();
                var refreshToken = RefreshTokenEntity.Create(refreshTokenValue, user.Id);

                // Revocar tokens anteriores y guardar nuevo
                await _refreshTokenRepository.RevokeAllUserTokensAsync(user.Id, cancellationToken);
                await _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken, cancellationToken);

                return LoginResult.Success(accessToken, refreshTokenValue, user.FullName, permissions);
            }
            catch (DomainException ex)
            {
                return LoginResult.Fail(ex.Message);
            }
        }

        public async Task<LoginResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                    return LoginResult.Fail("Refresh token es requerido");

                // Obtener token
                var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);
                if (storedToken == null)
                    return LoginResult.Fail("Refresh token inválido");

                // Validar (regla de negocio en el dominio)
                storedToken.ValidateForUse();

                // Obtener usuario
                var user = await _userRepository.GetUserByIdAsync(storedToken.UserId, cancellationToken);
                if (user == null)
                    return LoginResult.Fail("Usuario no encontrado");

                user.ValidateCanLogin();

                // Obtener permisos del rol del usuario
                var permissions = new List<UserPermissionResponse>();
                if (user.RoleId.HasValue)
                {
                    var rolePermissions = await _permissionRepository.GetRolePermissionsAsync(user.RoleId.Value, cancellationToken);
                    permissions = rolePermissions.Select(p => new UserPermissionResponse(
                        MenuCode: p.MenuCode ?? string.Empty,
                        ActionCode: p.ActionCode ?? string.Empty
                    )).ToList();
                }

                // Generar nuevos tokens con permisos incluidos en el JWT
                var permissionTuples = permissions.Select(p => (p.MenuCode, p.ActionCode));
                var newAccessToken = _jwtTokenGenerator.Generate(user, permissionTuples);
                var newRefreshTokenValue = _jwtTokenGenerator.GenerateRefreshToken();
                var newRefreshToken = RefreshTokenEntity.Create(newRefreshTokenValue, user.Id);

                // Revocar token anterior y guardar nuevo
                await _refreshTokenRepository.RevokeTokenAsync(refreshToken, cancellationToken);
                await _refreshTokenRepository.SaveRefreshTokenAsync(newRefreshToken, cancellationToken);

                return LoginResult.Success(newAccessToken, newRefreshTokenValue, user.FullName, permissions);
            }
            catch (DomainException ex)
            {
                return LoginResult.Fail(ex.Message);
            }
        }
    }
}