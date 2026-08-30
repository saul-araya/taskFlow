
using taskFlow.auth.Application.Dtos.Token;
using taskFlow.auth.Application.Dtos.User;

namespace taskFlow.auth.Application.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(ResUserDto dto);
    public RefreshTokenDto GenerateRefreshToken();
    public string GetTokenHash(string plainRefreshToken);
}
