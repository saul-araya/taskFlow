
using taskFlow.auth.Application.Dtos.UserProvider;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Application.Mappers.Interfaces;

public interface IUserProviderMapper
{
    UserProvider MapToEntity(CreateUserProviderDto dto);
    UserProvider MapToEntity(CreateUserProviderItemDto dto);
    ResUserProviderDto MapToDto(UserProvider dto);
}
