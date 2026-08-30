
namespace taskFlow.auth.Application.Dtos.Auth;

public record RefreshAccessTokenReqDto(
    string RefreshToken,
    Guid UserId
){}
