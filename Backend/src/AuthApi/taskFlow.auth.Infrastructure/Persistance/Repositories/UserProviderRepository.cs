
using taskFlow.auth.Domain.Entities;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Infrastructure.Persistance.Repositories;

public class UserProviderRepository(
    AppDbContext _context
) : IUserProviderRepository
{
    public async Task AddUserProviderAsync(UserProvider entity)
    {
        await _context.UserProviders.AddAsync(entity);
    }
}
