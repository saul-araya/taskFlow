
using taskFlow.auth.Application.Dtos.UserProvider;

namespace taskFlow.auth.Application.Dtos.User;

public record ResUserWithProviderDto(
    Guid Id,
    string Name,
    string DisplayName,
    string Email,
    string? ImageLink,
    bool Active
){
    public List<ResUserProviderDto> UserProviders { get; set; } = [];
}
