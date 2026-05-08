namespace DiskyNet.Application.Auth.Response
{
    public sealed record UserPermissionResponse(
       string MenuCode,
       string ActionCode
   );
}
