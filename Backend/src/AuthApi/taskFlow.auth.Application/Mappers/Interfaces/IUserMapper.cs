
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Application.Mappers.Interfaces;

public interface IUserMapper
{
    ResUserDto MapToDto(User entity);
    User MapToEntity(CreateUserDto dto);
    void MapToUpdate(User entity, UpdateUserDto dto);
}
