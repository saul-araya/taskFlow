
namespace taskFlow.auth.Application.Dtos.Auth;

public record LocalAuthRequestDto(
    string Email,
    string Password
){}
