
namespace taskFlow.auth.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid UserId { get; set; }
    public string RefreshTokenHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }

    public bool IsExpired() => ExpiresAt <= DateTime.UtcNow;

    //References
    public User User { get; set; } = null!;
}
