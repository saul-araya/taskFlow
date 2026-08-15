
using taskFlow.auth.Application.Interfaces;

namespace taskFlow.auth.Infrastructure.Persistance;

public class UnitOfWork(
    AppDbContext context
) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellation = default) => context.SaveChangesAsync(cancellation);
}
