
using taskFlow.auth.Application.Dtos.Token;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Application.Mappers.Implementations;

public class TokenMapper : ITokenMapper
{
    public RefreshToken ToEntity(RefreshTokenDto dto)
    {
        return new RefreshToken
        {
            RefreshTokenHash = dto.RefreshTokenHash,
            CreatedAt = dto.CreatedAt,
            ExpiresAt = dto.ExpiresAt,
            IsActive = dto.IsActive
        };
    }
}