
using taskFlow.auth.Domain.Enums;

namespace taskFlow.auth.Application.Dtos.UserProvider;

public record ResUserProviderDto(
    Guid Id,
    Guid UserId,
    AuthProvider Provider,
    string? ProviderUserId
){}
