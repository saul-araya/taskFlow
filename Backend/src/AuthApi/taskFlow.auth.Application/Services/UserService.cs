
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Domain.Entities;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Application.Services;

public class UserService : IUserService
{
    Task<ResUserDto> IUserService.AddUserAsync(CreateUserDto dto)
    {
        throw new NotImplementedException();
    }

    Task<ResUserDto?> IUserService.FindUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    Task<ResUserDto?> IUserService.FindUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    Task IUserService.SoftUserDeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    Task<ResUserDto> IUserService.UpdateUserAsync(UpdateUserDto dto)
    {
        throw new NotImplementedException();
    }
}
