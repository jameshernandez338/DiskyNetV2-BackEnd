using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DiskyNet.Application.Auth.Request;

namespace DiskyNet.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private const string RefreshTokenCookieName = "refreshToken";
        private const int RefreshTokenExpirationDays = 7;

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request, cancellationToken);

            if (!result.IsSuccess)
                return Unauthorized(new { message = result.Message });

            SetRefreshTokenCookie(result.RefreshToken!);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies[RefreshTokenCookieName];

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized(new { message = "Refresh token no encontrado" });

            var result = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);

            if (!result.IsSuccess)
            {
                RemoveRefreshTokenCookie();
                return Unauthorized(new { message = result.Message });
            }

            SetRefreshTokenCookie(result.RefreshToken!);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            RemoveRefreshTokenCookie();
            return Ok(new { message = "Sesión cerrada" });
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var isHttps = Request.IsHttps;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = isHttps,
                SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(RefreshTokenExpirationDays),
                Path = "/"
            };

            Response.Cookies.Append(RefreshTokenCookieName, refreshToken, cookieOptions);
        }

        private void RemoveRefreshTokenCookie()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                Path = "/"
            };

            Response.Cookies.Append(RefreshTokenCookieName, string.Empty, cookieOptions);
        }
    }
}