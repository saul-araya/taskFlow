using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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

        var publicKey = RSA.Create();

        publicKey.ImportFromPem(configuration["Jwt:PublicKey"] ?? throw new ArgumentException("Jwt:PublicKey is not configure"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],

                    IssuerSigningKey = new RsaSecurityKey(publicKey)
                };
            });

        //Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Services
        services.AddSingleton<IEncriptionService, EncriptionService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IAuthProvider<GoogleAuthRequestsDto>, GoogleAuthenticationService>();
        services.AddScoped<IAuthProvider<LocalAuthRequestDto>, LocalAuthenticationService>();

        return services;
    }
}
