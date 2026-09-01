
using taskFlow.auth.Domain.Enums;

namespace taskFlow.auth.Application.Dtos.UserProvider;

public record CreateUserProviderDto(
    Guid UserId,
    AuthProvider Provider,
    string? ProviderUserId,
    string? Password
)
{
}
