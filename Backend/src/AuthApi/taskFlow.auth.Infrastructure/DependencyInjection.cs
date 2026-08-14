using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using taskFlow.auth.Infrastructure.Persistance;

namespace taskFlow.auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("DefaultConnection string is not configured.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        return services;
    }
}
