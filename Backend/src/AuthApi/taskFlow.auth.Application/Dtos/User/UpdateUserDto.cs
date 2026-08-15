
namespace taskFlow.auth.Application.Dtos.User;

public record UpdateUserDto(
    string DisplayName,
    string Email,
    string? ImageLink
){}
