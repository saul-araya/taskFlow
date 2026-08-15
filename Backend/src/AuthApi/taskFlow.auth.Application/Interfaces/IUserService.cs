
using taskFlow.auth.Application.Dtos.User;

namespace taskFlow.auth.Application.Interfaces;

public interface IUserService
{
    Task<ResUserDto> AddUserAsync(CreateUserDto dto);
    Task<ResUserDto> UpdateUserAsync(UpdateUserDto dto, Guid id);
    Task<ResUserDto> FindUserByEmailAsync(string email);
    Task<ResUserDto> FindUserByIdAsync(Guid id);
    Task SoftUserDeleteAsync(Guid id);
}
