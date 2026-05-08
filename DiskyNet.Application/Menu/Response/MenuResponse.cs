namespace DiskyNet.Application.Menu.Response
{
    public sealed record MenuResponse(
        int Id,
        string MenuName,
        string? Code,
        string? MenuRoute,
        string? Icon,
        int DisplayOrder,
        int? ParentId,
        string MenuType
    );
}
