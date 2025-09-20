namespace DW.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string TokenValue { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedReason { get; set; }
    public string? ReplacedByToken { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsUsed => UsedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired && !IsUsed;
}