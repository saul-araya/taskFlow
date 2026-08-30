
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Dtos.Token;

namespace taskFlow.auth.Application.Interfaces;

public interface IAuthService
{
    public Task<AuthResDto> LocalAuthenticate(LocalAuthRequestDto dto);
    public Task<AuthResDto> GoogleAuthenticate(GoogleAuthRequestsDto dto);
    public Task<string> RefreshAccessToken(RefreshAccessTokenReqDto dto);
    public Task LogOut(ReqLogOutDto dto);
}
