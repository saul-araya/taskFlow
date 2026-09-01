
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Exceptions;
using taskFlow.auth.Application.Exceptions.Messages;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Domain.Entities;
using taskFlow.auth.Domain.Exceptions;
using taskFlow.auth.Domain.Exceptions.Messages;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Application.Services;

public class UserService(
    IEncriptionService encryptionService,
    IUserMapper mapper,
    IUserProviderMapper providerMapper,
    IUserRepository repository,
    IUnitOfWork unitOfWork
) : IUserService
{
    public async Task<ResUserDto> AddUserAsync(CreateUserDto dto)
    {
        var user = BuildUserWithProvider(dto);
        await repository.AddUserAsync(user);
        return mapper.MapToDto(user);
    }

    public async Task<ResUserDto> FindUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) 
            throw new BadRequestDataException(String.Format(ApplicationExceptionMessages.InvalidData, nameof(email)));
        var user = await repository.FindByEmailAsync(email) 
            ?? throw new NotFoundException(ApplicationExceptionMessages.NotFound);
        return mapper.MapToDto(user);
    }

    public async Task<ResUserWithProviderDto?> FindUserByEmailAndProvidersAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new BadRequestDataException(String.Format(ApplicationExceptionMessages.InvalidData, nameof(email)));
        var user = await repository.FindByEmailAndProvidersAsync(email);

        if(user == null) return null;

        var userDto = mapper.MapToDtoWithProvider(user);
        userDto.UserProviders = [.. user.UserProviders.Select(providerMapper.MapToDto)];
        return userDto;
    }

    public async Task<ResUserDto> FindUserByIdAsync(Guid id)
    {
        return mapper.MapToDto(await FindUserById(id));
    }

    public async Task SoftUserDeleteAsync(Guid id)
    {
        var user = await FindUserById(id);
        user.Active = false;
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<ResUserDto> UpdateUserAsync(UpdateUserDto dto, Guid id)
    {
        var user = await FindUserById(id);
        mapper.MapToUpdate(user, dto);
        await unitOfWork.SaveChangesAsync();
        return mapper.MapToDto(user);
    }

    private async Task<User> FindUserById(Guid id) => await repository.FindByIdAsync(id) ??
            throw new NotFoundException(ApplicationExceptionMessages.NotFound);

    private User BuildUserWithProvider(CreateUserDto dto)
    {
        var user = mapper.MapToEntity(dto);
        user.Id = Guid.CreateVersion7();
        var provider = providerMapper.MapToEntity(dto.UserProvider);

        if (dto.UserProvider.Provider == Domain.Enums.AuthProvider.LOCAL)
            provider.PasswordHash = encryptionService.EncryptPassword(dto.UserProvider.Password ?? 
                throw new InvalidPasswordException(DomainExceptionMessages.InvalidPassword));

        provider.Id = Guid.CreateVersion7();
        user.UserProviders.Add(provider);
        return user;
    }
}
