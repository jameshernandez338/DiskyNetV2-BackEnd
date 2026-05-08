namespace DiskyNet.Application.Permission.Response
{
    public sealed record MenuWithActionsResponse(
        int MenuId,
        string MenuCode,
        string MenuName,
        int DisplayOrder,
        int? ParentId,
        string? ParentName,
        List<ActionResponse> AvailableActions
    );
}
