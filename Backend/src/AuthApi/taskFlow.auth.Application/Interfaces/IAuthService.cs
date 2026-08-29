
using taskFlow.auth.Application.Dtos.Auth;

namespace taskFlow.auth.Application.Interfaces;

public interface IAuthService
{
    public Task<AuthResDto> LocalAuthenticate(LocalAuthRequestDto dto);
    public Task<AuthResDto> GoogleAuthenticate(GoogleAuthRequestsDto dto);
}
