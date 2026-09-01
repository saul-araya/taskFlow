
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Domain.Enums;

namespace taskFlow.auth.Application.Interfaces;

public interface IUserService
{
    Task<ResUserDto> AddUserAsync(CreateUserDto dto);
    Task<ResUserDto> UpdateUserAsync(UpdateUserDto dto, Guid id);
    Task<ResUserDto> FindUserByEmailAsync(string email);
    Task<ResUserWithProviderDto?> FindUserByEmailAndProvidersAsync(string email);
    Task<ResUserDto> FindUserByIdAsync(Guid id);
    Task SoftUserDeleteAsync(Guid id);
}
