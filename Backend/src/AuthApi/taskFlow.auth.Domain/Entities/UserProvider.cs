
using taskFlow.auth.Domain.Enums;

namespace taskFlow.auth.Domain.Entities;

public class UserProvider
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AuthProvider  Provider {  get; set; }
    public string? ProviderUserId { get; set; }
    public string? PasswordHash { get; set; }

    public User User { get; set; } = null!;
}
