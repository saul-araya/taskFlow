
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Domain.Entities;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Infrastructure.Services;

public class LocalAuthenticationService(
    IUserRepository _userRepository,
    IEncriptionService _encryption
) : IAuthProvider<LocalAuthRequestDto>
{
    public async Task<AuthResultDto> Authenticate(LocalAuthRequestDto dto)
    {
        var user = await _userRepository.FindByEmailAsync(dto.Email);

        if (user == null) return BuildAuthResult(false, null);

        var localProvider = user.UserProviders.FirstOrDefault(x => x.Provider == Domain.Enums.AuthProvider.LOCAL);

        if(localProvider == null || !_encryption.CompareEncryption(dto.Password, localProvider.PasswordHash!))
            return BuildAuthResult(false, null);

        return BuildAuthResult(true, user); 
    }

    private AuthResultDto BuildAuthResult(bool result, User? user)
    {
        return new AuthResultDto(
            result,
            user == null ? null : new ResUserDto(
                Id: user.Id,
                Name: user.Name,
                DisplayName: user.DisplayName,
                Email: user.Email,
                ImageLink: user.ImageLink,
                Active: user.Active
            )
        );
    }
}
