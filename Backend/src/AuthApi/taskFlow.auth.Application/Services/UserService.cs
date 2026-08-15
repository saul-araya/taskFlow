
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Domain.Entities;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Application.Services;

public class UserService(
    IEncriptionService encryptionService,
    IUserMapper mapper,
    IUserProviderMapper providerMapper,
    IUserRepository repository
) : IUserService
{
    public async Task<ResUserDto> AddUserAsync(CreateUserDto dto)
    {
        var user = mapper.MapToEntity(dto);

    }

    public async Task<ResUserDto?> FindUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<ResUserDto?> FindUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task SoftUserDeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResUserDto> UpdateUserAsync(UpdateUserDto dto)
    {
        throw new NotImplementedException();
    }
}
