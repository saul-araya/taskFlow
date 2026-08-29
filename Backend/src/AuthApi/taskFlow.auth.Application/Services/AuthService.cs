
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Exceptions;
using taskFlow.auth.Application.Exceptions.Messages;
using taskFlow.auth.Application.Interfaces;

namespace taskFlow.auth.Application.Services;

public class AuthService(
    IAuthProvider<GoogleAuthRequestsDto> _googleAuth,
    IAuthProvider<LocalAuthRequestDto> _localAuth
) : IAuthService
{
    public async Task<AuthResDto> GoogleAuthenticate(GoogleAuthRequestsDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthResDto> LocalAuthenticate(LocalAuthRequestDto dto)
    {
        var authResult = await _localAuth.Authenticate(dto);
        if (!authResult.IsSuccess || authResult.User == null)
            throw new InvalidCredentialsException(ApplicationExceptionMessages.InvalidCredentials);
        
        
    }
}
