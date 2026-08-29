
using taskFlow.auth.Application.Dtos.User;

namespace taskFlow.auth.Application.Dtos.Auth;

public record AuthResultDto(
    bool IsSuccess,
    ResUserDto? User
){}
