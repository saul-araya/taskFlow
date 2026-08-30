
using Microsoft.Extensions.DependencyInjection;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Application.Mappers.Implementations;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Application.Services;

namespace taskFlow.auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
    {   
        //Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        //Mappers
        services.AddSingleton<IUserMapper, UserMapper>();
        services.AddSingleton<IUserProviderMapper, UserProviderMapper>();
        services.AddSingleton<ITokenMapper, TokenMapper>();

        return services;
    }
}
