using taskFlow.auth.Application.Dtos.UserProvider;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Application.Mappers.Implementations;

public class UserProviderMapper : IUserProviderMapper
{
    public UserProvider MapToEntity(CreateUserProviderItemDto dto)
    {
        return new UserProvider
        {
            Provider = dto.Provider,
            ProviderUserId = dto.ProviderUserId,
        };
    }
}