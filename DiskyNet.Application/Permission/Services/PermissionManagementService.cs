using DiskyNet.Application.Permission.Interfaces;
using DiskyNet.Application.Permission.Request;
using DiskyNet.Application.Permission.Response;
using DiskyNet.Domain.Exceptions;
using DiskyNet.Domain.Menu.Interfaces;
using DiskyNet.Domain.Permission.Interfaces;
using DiskyNet.Domain.Role.Interfaces;

namespace DiskyNet.Application.Permission.Services
{
    public class PermissionManagementService : IPermissionManagementService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleRepository _roleRepository;

        public PermissionManagementService(
            IPermissionRepository permissionRepository,
            IMenuRepository menuRepository,
            IRoleRepository roleRepository)
        {
            _permissionRepository = permissionRepository;
            _menuRepository = menuRepository;
            _roleRepository = roleRepository;
        }

        public async Task<List<MenuWithActionsResponse>> GetMenusWithActionsAsync(CancellationToken cancellationToken)
        {
            // Obtener todos los menús
            var menus = await _menuRepository.GetAllMenusAsync(cancellationToken);
            var menusList = menus.ToList();

            // Crear diccionario para búsqueda rápida de nombres de menús
            var menuNamesDict = menusList.ToDictionary(m => m.Id, m => m.MenuName);

            // Obtener todas las relaciones MenuActions
            var menuActionsDict = await _permissionRepository.GetAllMenuActionsAsync(cancellationToken);

            // Obtener todas las acciones
            var allActions = await _permissionRepository.GetAllActionsAsync(cancellationToken);
            var actionsDict = allActions.ToDictionary(a => a.Id);

            var result = new List<MenuWithActionsResponse>();

            foreach (var menu in menusList)
            {
                // Obtener las acciones disponibles para este menú
                if (menuActionsDict.TryGetValue(menu.Id, out var actionIds))
                {
                    var availableActions = actionIds
                        .Where(actionId => actionsDict.ContainsKey(actionId))
                        .Select(actionId => {
                            var action = actionsDict[actionId];
                            return new ActionResponse(
                                Id: action.Id,
                                Code: action.Code,
                                Description: action.Description
                            );
                        })
                        .ToList();

                    // Obtener el nombre del padre si existe
                    string? parentName = null;
                    if (menu.ParentId.HasValue && menuNamesDict.TryGetValue(menu.ParentId.Value, out var pName))
                    {
                        parentName = pName;
                    }

                    result.Add(new MenuWithActionsResponse(
                        MenuId: menu.Id,
                        MenuCode: menu.Code ?? string.Empty,
                        MenuName: menu.MenuName,
                        DisplayOrder: menu.DisplayOrder,
                        ParentId: menu.ParentId,
                        ParentName: parentName,
                        AvailableActions: availableActions
                    ));
                }
            }

            // Ordenar por DisplayOrder (orden configurado en BD)
            return result.OrderBy(m => m.DisplayOrder).ToList();
        }

        public async Task<RolePermissionsManagementResponse> GetRolePermissionsForManagementAsync(int roleId, CancellationToken cancellationToken)
        {
            // Validar que el rol existe
            var role = await _roleRepository.GetRoleByIdAsync(roleId, cancellationToken);
            if (role == null)
                throw new DomainException($"Role with ID {roleId} not found");

            // Obtener los permisos actuales del rol
            var currentPermissions = await _permissionRepository.GetRolePermissionsAsync(roleId, cancellationToken);
            var permissionsSet = currentPermissions
                .Select(p => (p.MenuId, p.ActionId))
                .ToHashSet();

            // Obtener todos los menús con sus acciones
            var menusWithActions = await GetMenusWithActionsAsync(cancellationToken);

            // Obtener todos los menús para construir el diccionario con DisplayOrder
            var allMenus = await _menuRepository.GetAllMenusAsync(cancellationToken);
            var menuDisplayOrderDict = allMenus.ToDictionary(m => m.Id, m => m.DisplayOrder);

            // Construir la respuesta con información de qué está permitido y qué no
            // Ya viene ordenado por DisplayOrder desde GetMenusWithActionsAsync
            var menuPermissions = menusWithActions.Select(menu =>
            {
                // Obtener el DisplayOrder del padre si existe
                int? parentDisplayOrder = null;
                if (menu.ParentId.HasValue && menuDisplayOrderDict.TryGetValue(menu.ParentId.Value, out var pDisplayOrder))
                {
                    parentDisplayOrder = pDisplayOrder;
                }

                return new MenuPermissionResponse(
                    MenuId: menu.MenuId,
                    MenuCode: menu.MenuCode,
                    MenuName: menu.MenuName,
                    DisplayOrder: menu.DisplayOrder,
                    ParentId: menu.ParentId,
                    ParentName: menu.ParentName,
                    ParentDisplayOrder: parentDisplayOrder,
                    Actions: menu.AvailableActions.Select(action => new ActionPermissionResponse(
                        ActionId: action.Id,
                        ActionCode: action.Code,
                        ActionDescription: action.Description,
                        IsGranted: permissionsSet.Contains((menu.MenuId, action.Id))
                    )).ToList()
                );
            }).ToList();

            return new RolePermissionsManagementResponse(
                RoleId: roleId,
                RoleName: role.Name,
                MenuPermissions: menuPermissions
            );
        }

        public async Task UpdateRolePermissionsAsync(int roleId, UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
        {
            // Validar que el rol existe
            var role = await _roleRepository.GetRoleByIdAsync(roleId, cancellationToken);
            if (role == null)
                throw new DomainException($"Role with ID {roleId} not found");

            // Eliminar todos los permisos actuales del rol
            await _permissionRepository.DeleteAllRolePermissionsAsync(roleId, cancellationToken);

            // Crear solo los permisos que están marcados como granted
            var permissionsToCreate = request.Permissions.Where(p => p.IsGranted).ToList();

            foreach (var permission in permissionsToCreate)
            {
                await _permissionRepository.CreateRolePermissionAsync(
                    roleId,
                    permission.MenuId,
                    permission.ActionId,
                    cancellationToken
                );
            }
        }
    }
}
