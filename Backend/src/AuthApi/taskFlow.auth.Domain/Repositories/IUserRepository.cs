
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Domain.Repositories;

public interface IUserRepository
{
    Task<User> AddUserAsync(User entity);
    Task<User?> FindByIdAsync(Guid id);
    Task<User?> FindByEmailAsync(string email);
}
