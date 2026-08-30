
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using taskFlow.auth.Application.Dtos.Token;
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Infrastructure.Services;

public class TokenService(
    IConfiguration _configuration,
    IEncriptionService _encryption
) : ITokenService
{
    private readonly string Issuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentException("Jwt:Issuer is not configure");
    private readonly string PrivateKey = _configuration["Jwt:PrivateKey"] ?? throw new ArgumentException("Jwt:PrivateKey is not configure");
    private readonly string Audience = _configuration["Jwt:Audience"] ?? throw new ArgumentException("Jwt:Audience is not configure");
    private readonly string AccessTokenMinutesTime = _configuration["Jwt:AccessTokenMinutesTime"] ?? throw new ArgumentException("Jwt:AccessTokenMinutesTime is not configure");
    private readonly string RefreshTokenDaysTime = _configuration["Jwt:RefreshTokenDaysTime"] ?? throw new ArgumentException("Jwt:RefreshTokenDaysTime is not configure");

    public string GenerateAccessToken(ResUserDto dto)
    {
        var privateKey = RSA.Create();
        privateKey.ImportFromPem(PrivateKey);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, dto.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, dto.Email),
            new (ClaimTypes.Name, dto.Name)
        };

        var credentials = new SigningCredentials(
            new RsaSecurityKey(privateKey),
            SecurityAlgorithms.RsaSha256
        );

        int minutes = int.TryParse(AccessTokenMinutesTime, out var d) ? d : 15;

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(minutes)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshTokenDto GenerateRefreshToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        var token = Base64Url.EncodeToString(tokenBytes);
        var refreshTokenHash = SHA256.HashData(tokenBytes);
        var refrshTokenBase64 = Base64Url.EncodeToString(refreshTokenHash);

        int days = int.TryParse(RefreshTokenDaysTime, out var d) ? d : 30;
        var now = DateTime.UtcNow;

        return new RefreshTokenDto(
            token,
            refrshTokenBase64,
            now,
            now.AddDays(days),
            IsActive: true
        );
    }

    public string GetTokenHash(string plainRefreshToken)
    {
        var tokenBytes = Base64Url.DecodeFromChars(plainRefreshToken);
        var tokenHash = SHA256.HashData(tokenBytes);
        return Base64Url.EncodeToString(tokenHash);
    }
}
