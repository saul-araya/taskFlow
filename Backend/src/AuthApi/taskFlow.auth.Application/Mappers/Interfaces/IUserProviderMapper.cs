
using taskFlow.auth.Application.Dtos.UserProvider;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Application.Mappers.Interfaces;

public interface IUserProviderMapper
{
    UserProvider MapToEntity(CreateUserProviderItemDto dto);
}
