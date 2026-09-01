
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Dtos.Token;
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Dtos.UserProvider;
using taskFlow.auth.Application.Exceptions;
using taskFlow.auth.Application.Exceptions.Messages;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Domain.Enums;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Application.Services;

public class AuthService(
    IAuthProvider<GoogleAuthRequestsDto, GoogleValidationResultDto> _googleAuth,
    IAuthProvider<LocalAuthRequestDto, AuthResultDto> _localAuth,
    IRefreshTokenRepository _refreshTokenRepository,
    IJWTService _token,
    ITokenMapper _tokenMapper,
    IUnitOfWork unitOfWork,
    IUserService _userService,
    IUserProviderService _providerService
) : IAuthService
{
    public async Task<AuthResDto> GoogleAuthenticate(GoogleAuthRequestsDto dto)
    {
        var googleValidationResult = await _googleAuth.Authenticate(dto);
        if (!googleValidationResult.IsSuccesfull) 
            throw new UnauthorizedException(ApplicationExceptionMessages.UnauthorizedGoogleIdToken);

        var userDto = await _userService.FindUserByEmailAndProvidersAsync(googleValidationResult.Email!);

        if(userDto == null)
        {
            var newUser = BuildCreateUserDto(googleValidationResult, AuthProvider.GOOGLE);
            var createdUser = await _userService.AddUserAsync(newUser);
            var (accessToken, refreshToken) = await BuildAndSaveTokens(createdUser);
            return BuildAuthResDto(accessToken, refreshToken, createdUser);
        }

        var hasGoogleProvider = userDto.UserProviders.Any(x => x.Provider == AuthProvider.GOOGLE);
        var userResDto = new ResUserDto(userDto.Id, userDto.Name, userDto.DisplayName, userDto.Email, userDto.ImageLink, userDto.Active);

        if (!hasGoogleProvider)
        {
            await _providerService.AddUserProviderAsync(new CreateUserProviderDto(UserId: userDto.Id, AuthProvider.GOOGLE, googleValidationResult.ProviderUserId, null));
            var (accessToken, refreshToken) = await BuildAndSaveTokens(userResDto);
            return BuildAuthResDto(accessToken, refreshToken, userResDto);
        }

        var (newAccessToken, newRefreshToken) = await BuildAndSaveTokens(userResDto);
        return BuildAuthResDto(newAccessToken, newRefreshToken, userResDto);
    }

    public async Task<AuthResDto> LocalAuthenticate(LocalAuthRequestDto dto)
    {
        var authResult = await _localAuth.Authenticate(dto);
        if (!authResult.IsSuccess || authResult.User == null)
            throw new InvalidCredentialsException(ApplicationExceptionMessages.InvalidCredentials);

        var (accessToken, refreshToken) = await BuildAndSaveTokens(authResult.User);
        return BuildAuthResDto(accessToken, refreshToken, authResult.User);
    }

    public async Task<AccessTokenDto> RefreshAccessToken(RefreshAccessTokenReqDto dto)
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

        var user= userTokenData.User;
        var userResDto = new ResUserDto(user.Id, user.Name, user.DisplayName, user.Email, user.ImageLink, user.Active);
        return new AccessTokenDto(_token.GenerateAccessToken(userResDto));
    }

    public async Task<AuthResDto> LocalUserRegister(CreateUserDto dto)
    {
        var createdUser = await _userService.AddUserAsync(dto);
        var (accessToken, refreshToken) = await BuildAndSaveTokens(createdUser);
        return BuildAuthResDto(accessToken, refreshToken, createdUser);
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

    private static CreateUserDto BuildCreateUserDto(GoogleValidationResultDto dto, AuthProvider provider, string? password = null) => new(dto.Name!,
                    dto.DisplayName!,
                    dto.Email!,
                    dto.ImageLink,
                    new Dtos.UserProvider.CreateUserProviderItemDto(
                        AuthProvider.GOOGLE,
                        dto.ProviderUserId,
                        null
                    )
                );

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
