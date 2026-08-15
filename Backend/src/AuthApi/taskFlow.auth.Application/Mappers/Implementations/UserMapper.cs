using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Application.Mappers.Implementations;

public class UserMapper(
    IUserProviderMapper providerMapper
) : IUserMapper
{
    public ResUserDto MapToDto(User entity)
    {
        return new ResUserDto(
            Id: entity.Id,
            Name: entity.Name,
            DisplayName: entity.DisplayName,
            Email: entity.Email,
            ImageLink: entity.ImageLink,
            Active: entity.Active
        );
    }

    public User MapToEntity(CreateUserDto dto)
    {
        return new User
        {
            Name = dto.Name,
            DisplayName = dto.DisplayName,
            Email = dto.Email,
            ImageLink = dto.ImageLink,
            Active = true,
            UserProviders = [.. dto.UserProviders.Select(providerMapper.MapToEntity)]
        };
    }

    public User MapToUpdate(User entity, UpdateUserDto dto)
    {
        entity.DisplayName = dto.DisplayName;
        entity.Email = dto.Email;
        entity.ImageLink = dto.ImageLink;
        return entity;
    }
}
