
using taskFlow.auth.Application.Dtos.Token;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Application.Mappers.Interfaces;

public interface ITokenMapper
{
    RefreshToken ToEntity(RefreshTokenDto dto);
}
