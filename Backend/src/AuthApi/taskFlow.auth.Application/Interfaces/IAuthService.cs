
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Dtos.Token;
using taskFlow.auth.Application.Dtos.User;

namespace taskFlow.auth.Application.Interfaces;

public interface IAuthService
{
    public Task<AuthResDto> LocalAuthenticate(LocalAuthRequestDto dto);
    public Task<AuthResDto> GoogleAuthenticate(GoogleAuthRequestsDto dto);
    public Task<AccessTokenDto> RefreshAccessToken(RefreshAccessTokenReqDto dto);
    public Task<AuthResDto> LocalUserRegister(CreateUserDto dto);
    public Task LogOut(ReqLogOutDto dto);
}
