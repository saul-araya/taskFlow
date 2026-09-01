using taskFlow.auth.Application.Dtos.UserProvider;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Application.Mappers.Implementations;

public class UserProviderMapper : IUserProviderMapper
{
    public UserProvider MapToEntity(CreateUserProviderDto dto)
    {
        return new UserProvider
        {
            UserId = dto.UserId,
            Provider = dto.Provider,
            ProviderUserId = dto.ProviderUserId
        };
    }

    public UserProvider MapToEntity(CreateUserProviderItemDto dto)
    {
        return new UserProvider
        {
            Provider = dto.Provider,
            ProviderUserId = dto.ProviderUserId,
        };
    }

    public ResUserProviderDto MapToDto(UserProvider entity)
    {
        return new ResUserProviderDto(
            Id: entity.Id,
            UserId: entity.UserId,
            Provider: entity.Provider,
            ProviderUserId: entity.ProviderUserId
        );
    }

}