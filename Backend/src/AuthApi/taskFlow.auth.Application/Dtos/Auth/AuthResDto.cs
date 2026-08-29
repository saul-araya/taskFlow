
using taskFlow.auth.Application.Dtos.User;

namespace taskFlow.auth.Application.Dtos.Auth;

public record AuthResDto(
    string AccessToken,
    string RefreshToken,
    ResUserDto UserData
){}
