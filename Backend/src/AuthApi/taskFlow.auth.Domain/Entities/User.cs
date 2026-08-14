
namespace taskFlow.auth.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ImageLink { get; set; }

    public List<UserProvider> UserProviders { get; set; } = []; 
}
