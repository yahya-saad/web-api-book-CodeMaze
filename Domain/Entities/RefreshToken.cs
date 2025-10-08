namespace Domain.Entities;
public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public bool IsUsed { get; set; } = false;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsExpired && !IsRevoked && !IsUsed;

    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
}

