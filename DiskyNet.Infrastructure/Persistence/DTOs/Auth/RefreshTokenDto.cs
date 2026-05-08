namespace DiskyNet.Infrastructure.Persistence.DTOs.Auth
{
    public class RefreshTokenDto
    {
        public string Token { get; set; } = default!;
        public long UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
