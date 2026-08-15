
using Microsoft.EntityFrameworkCore;
using taskFlow.auth.Domain.Entities;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Infrastructure.Persistance.Repositories;

public class UserRepository(
    AppDbContext context
) : IUserRepository
{
    public async Task<User> AddUserAsync(User entity)
    {
        await context.Users.AddAsync(entity);
        return entity;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await context.Users
            .Include(x => x.UserProviders)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> FindByIdAsync(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}
