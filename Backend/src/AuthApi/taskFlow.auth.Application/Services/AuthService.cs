
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Dtos.Token;
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
    IJWTService _token,
    ITokenMapper _tokenMapper,
    IUnitOfWork unitOfWork,
    IUserMapper _userMapper
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

        var (accessToken, refreshToken) = await BuildAndSaveTokens(authResult.User);
        return BuildAuthResDto(accessToken, refreshToken, authResult.User);
    }

    public async Task<string> RefreshAccessToken(RefreshAccessTokenReqDto dto)
    {
        var tokenHash = _token.GetTokenHash(dto.RefreshToken);
        var userTokenData = await _refreshTokenRepository.FindRefreshTokenAndUserAsync(tokenHash, dto.UserId) 
            ?? throw new UnauthorizedException(ApplicationExceptionMessages.UnauthorizedRefreshToken);

        if (userTokenData.IsExpired() || !userTokenData.IsActive)
        {
            userTokenData.IsActive = false;
            await unitOfWork.SaveChangesAsync();
            throw new UnauthorizedException(ApplicationExceptionMessages.UnauthorizedRefreshToken);
        }

        var userResDto = _userMapper.MapToDto(userTokenData.User);
        return _token.GenerateAccessToken(userResDto);
    }

    public async Task LogOut(ReqLogOutDto dto)
    {
        var tokenHash = _token.GetTokenHash(dto.RefreshToken);
        var refreshTokenDb = await _refreshTokenRepository.FindRefreshTokenAndUserAsync(tokenHash, dto.UserId);

        if (refreshTokenDb == null || !refreshTokenDb.IsActive) return;

        refreshTokenDb.IsActive = false;
        await unitOfWork.SaveChangesAsync();
    }

    private static AuthResDto BuildAuthResDto(string accessToken, string refreshToken, ResUserDto user) => new(accessToken, refreshToken, user);

    private async Task<(string accessToken, string refreshToken)> BuildAndSaveTokens(ResUserDto user)
    {
        var accessToken = _token.GenerateAccessToken(user);
        var refreshTokenDto = _token.GenerateRefreshToken();
        var refreshToken = _tokenMapper.ToEntity(refreshTokenDto);

        refreshToken.UserId = user.Id;

        await _refreshTokenRepository.AddRefreshToken(refreshToken);
        await unitOfWork.SaveChangesAsync();

        return (accessToken, refreshTokenDto.token);
    }
}
