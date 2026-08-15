
namespace taskFlow.auth.Application.Dtos.UserProvider;

public record CreateUserProviderItemDto(
    string Provider,
    string? ProviderUserId,
    string? Password
){ }
