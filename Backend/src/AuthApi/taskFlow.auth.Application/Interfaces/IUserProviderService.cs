
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Dtos.UserProvider;

namespace taskFlow.auth.Application.Interfaces;

public interface IUserProviderService
{
    Task<ResUserProviderDto> AddUserProviderAsync(CreateUserProviderDto dto);
}
