
namespace taskFlow.auth.Application.Dtos.User;

public record ResUserDto(
    Guid Id,
    string Name,
    string DisplayName,
    string Email,
    string? ImageLink,
    bool Active
){}
