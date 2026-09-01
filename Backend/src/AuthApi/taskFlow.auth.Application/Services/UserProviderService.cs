
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Dtos.UserProvider;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Application.Services;

public class UserProviderService(
    IUserProviderRepository _repository,
    IUserProviderMapper _mapper
) : IUserProviderService
{
    public async Task<ResUserProviderDto> AddUserProviderAsync(CreateUserProviderDto dto)
    {
        var userProvider = _mapper.MapToEntity(dto);
        await _repository.AddUserProviderAsync(userProvider);
        return _mapper.MapToDto(userProvider);
    }
}
