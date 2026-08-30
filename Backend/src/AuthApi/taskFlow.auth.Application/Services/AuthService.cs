
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Exceptions;
using taskFlow.auth.Application.Exceptions.Messages;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Application.Services;

public class AuthService(
    IAuthProvider<GoogleAuthRequestsDto> _googleAuth,
    IAuthProvider<LocalAuthRequestDto> _localAuth,
    IRefreshTokenRepository _refreshTokenRepository,
    ITokenService _token,
    ITokenMapper _tokenMapper,
    IUnitOfWork unitOfWork
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

        var accessToken = _token.GenerateAccessToken(authResult.User);
        var refreshTokenDto = _token.GenerateRefreshToken();
        var refreshToken = _tokenMapper.ToEntity(refreshTokenDto);

        refreshToken.UserId = authResult.User.Id;

        await _refreshTokenRepository.AddRefreshToken(refreshToken);
        await unitOfWork.SaveChangesAsync();

        return BuildAuthResDto(accessToken, refreshTokenDto.token, authResult.User);
    }

    public async Task LogOut(ReqLogOutDto dto)
    {
        var tokenHash = _token.GetTokenHash(dto.refreshToken);
        var refreshTokenDb = await _refreshTokenRepository.FindRefreshTokenByHash(tokenHash);

        if (refreshTokenDb == null) return;

        refreshTokenDb.IsActive = false;
        await unitOfWork.SaveChangesAsync();
    }

    private static AuthResDto BuildAuthResDto(string accessToken, string refreshToken, ResUserDto user) => new(accessToken, refreshToken, user);
}
