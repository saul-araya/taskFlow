
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Interfaces;

namespace taskFlow.auth.Infrastructure.Services;

public class GoogleAuthenticationService(
    IConfiguration _configuration
) : IAuthProvider<GoogleAuthRequestsDto, GoogleValidationResultDto>
{
    private readonly string googleClientId = _configuration["Google:ClientId"] ?? throw new ArgumentException("Google Client Id is not configure.");
    public async Task<GoogleValidationResultDto> Authenticate(GoogleAuthRequestsDto dto)
    {
        var googleSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [googleClientId]
        };
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.GoogleIdToken, googleSettings);
            return BuildValidationResult(
                true,
                payload.Name,
                payload.Name,
                payload.Email,
                payload.Picture,
                payload.Subject
            );    
        }
        catch (Exception)
        {
            return BuildValidationResult(false);
        }
    }

    private GoogleValidationResultDto BuildValidationResult(
        bool IsSuccesfull,
        string? Name = null,
        string? DisplayName = null,
        string? Email = null,
        string? ImageLink = null,
        string? providerUserId = null
    ) => new(
        IsSuccesfull,
        Name,
        DisplayName,
        Email,
        ImageLink,
        providerUserId
    );
}
