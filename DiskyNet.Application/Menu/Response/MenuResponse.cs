namespace DiskyNet.Application.Menu.Response
{
    public record MenuResponse(
        int Id,
        string MenuName,
        string? MenuRoute,
        string? Icon,
        int DisplayOrder,
        int? ParentId,
        string MenuType
    );
}
