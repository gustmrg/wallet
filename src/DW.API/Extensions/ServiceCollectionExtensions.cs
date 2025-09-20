using DW.Application.Interfaces;
using DW.Infrastructure;
using DW.Infrastructure.Options;
using DW.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace DW.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException("Connection string not found");
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        
        return services;
    }
}