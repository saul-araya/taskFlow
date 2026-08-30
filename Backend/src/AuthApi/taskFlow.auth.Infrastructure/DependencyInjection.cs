using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using taskFlow.auth.Application.Dtos.Auth;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Domain.Repositories;
using taskFlow.auth.Infrastructure.Persistance;
using taskFlow.auth.Infrastructure.Persistance.Repositories;
using taskFlow.auth.Infrastructure.Services;

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

        //Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Services
        services.AddSingleton<IEncriptionService, EncriptionService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IAuthProvider<GoogleAuthRequestsDto>, GoogleAuthenticationService>();
        services.AddScoped<IAuthProvider<LocalAuthRequestDto>, LocalAuthenticationService>();

        return services;
    }
}
