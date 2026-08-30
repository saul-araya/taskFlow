
namespace taskFlow.auth.Application.Dtos.Token;

public record RefreshAndAccessTokenDto(
    string AccessToken,
    string? RefreshToken
){}
