
using taskFlow.auth.Domain.Enums;

namespace taskFlow.auth.Application.Dtos.UserProvider;

public record CreateUserProviderItemDto(
    AuthProvider Provider,
    string? ProviderUserId,
    string? Password
){ }
