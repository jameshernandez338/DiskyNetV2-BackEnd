using System.Security.Claims;

namespace DiskyNet.Api.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetId()
        {
            return _httpContextAccessor.HttpContext?.User
                ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public string? GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User
                ?.FindFirst("UserName")?.Value;
        }

        public string? GetEmail()
        {
            return _httpContextAccessor.HttpContext?.User
                ?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string? GetCompanyCode()
        {
            return _httpContextAccessor.HttpContext?.User
                ?.FindFirst("CompanyCode")?.Value;
        }
    }
}

