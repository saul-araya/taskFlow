
using taskFlow.auth.Domain.Entities;
using taskFlow.auth.Domain.Enums;

namespace taskFlow.auth.Domain.Repositories;

public interface IUserRepository
{
    Task<User> AddUserAsync(User entity);
    Task<User?> FindByIdAsync(Guid id);
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByEmailAndProvidersAsync(string email);
}
