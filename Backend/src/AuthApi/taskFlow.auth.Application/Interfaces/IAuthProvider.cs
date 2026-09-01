
using taskFlow.auth.Application.Dtos.Auth;

namespace taskFlow.auth.Application.Interfaces;

public interface IAuthProvider<T, K> where T : class
{
    public Task<K> Authenticate(T dto);
}
