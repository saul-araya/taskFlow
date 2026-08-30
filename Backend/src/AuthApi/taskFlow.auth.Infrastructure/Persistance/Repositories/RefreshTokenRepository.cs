
using Microsoft.EntityFrameworkCore;
using taskFlow.auth.Domain.Entities;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.Infrastructure.Persistance.Repositories;

public class RefreshTokenRepository(
    AppDbContext _context
) : IRefreshTokenRepository
{
    public async Task AddRefreshToken(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> FindRefreshTokenByHash(string tokenHash)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.RefreshTokenHash == tokenHash && x.IsActive);
    }
}
