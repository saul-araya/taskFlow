
using Microsoft.Extensions.DependencyInjection;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Application.Services;

namespace taskFlow.auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
