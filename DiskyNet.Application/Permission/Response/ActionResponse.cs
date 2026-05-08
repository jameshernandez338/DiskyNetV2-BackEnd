namespace DiskyNet.Application.Permission.Response
{
    public sealed record ActionResponse(
       int Id,
       string Code,
       string? Description
   );
}
