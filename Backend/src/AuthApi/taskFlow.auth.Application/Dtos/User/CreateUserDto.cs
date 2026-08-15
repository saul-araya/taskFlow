
using taskFlow.auth.Application.Dtos.UserProvider;

namespace taskFlow.auth.Application.Dtos.User;

public record CreateUserDto(
    string Name,
    string DisplayName,
    string Email,
    string? ImageLink,
    List<CreateUserProviderItemDto> UserProviders
){}
