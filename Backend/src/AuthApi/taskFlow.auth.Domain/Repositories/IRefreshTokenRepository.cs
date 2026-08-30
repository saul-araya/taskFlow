
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> FindRefreshTokenByHash(string tokenHash);
    Task AddRefreshToken(RefreshToken refreshToken);
}
