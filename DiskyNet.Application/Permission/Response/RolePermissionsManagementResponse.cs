namespace DiskyNet.Application.Permission.Response
{
    public sealed record RolePermissionsManagementResponse(
        int RoleId,
        string RoleName,
        List<MenuPermissionResponse> MenuPermissions
    );

    public sealed record MenuPermissionResponse(
        int MenuId,
        string MenuCode,
        string MenuName,
        int DisplayOrder,
        int? ParentId,
        string? ParentName,
        int? ParentDisplayOrder,
        List<ActionPermissionResponse> Actions
    );

    public sealed record ActionPermissionResponse(
        int ActionId,
        string ActionCode,
        string? ActionDescription,
        bool IsGranted
    );
}

