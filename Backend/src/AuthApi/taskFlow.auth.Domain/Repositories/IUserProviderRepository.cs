
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Domain.Repositories;

public interface IUserProviderRepository
{
    Task AddUserProviderAsync(UserProvider entity);
}
