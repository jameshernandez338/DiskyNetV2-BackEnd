using DiskyNet.Application.Auth.Response;
using System.Text.Json.Serialization;

namespace DiskyNet.Application.Auth.Reponse
{
    public class LoginResult
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public string? Token { get; }
        public string? FullName { get; }
        public IEnumerable<UserPermissionResponse>? Permissions { get; }

        // Exists internally but never serialized to JSON responses
        [JsonIgnore]
        public string? RefreshToken { get; }

        private LoginResult(
            bool success,
            string message,
            string? token = null,
            string? refreshToken = null,
            string? fullName = null,
            IEnumerable<UserPermissionResponse>? permissions = null)
        {
            IsSuccess = success;
            Message = message;
            Token = token;
            RefreshToken = refreshToken;
            FullName = fullName;
            Permissions = permissions;
        }

        public static LoginResult Fail(string message)
            => new LoginResult(false, message);

        public static LoginResult Success(
            string token,
            string refreshToken,
            string fullName,
            IEnumerable<UserPermissionResponse> permissions)
            => new LoginResult(true, "", token, refreshToken, fullName, permissions);
    }
}
