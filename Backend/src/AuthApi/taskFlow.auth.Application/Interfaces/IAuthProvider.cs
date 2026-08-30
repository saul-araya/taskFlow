
using taskFlow.auth.Application.Dtos.Auth;

namespace taskFlow.auth.Application.Interfaces;

public interface IAuthProvider<T> where T: class
{
    public Task<AuthResultDto> Authenticate(T dto);
}
