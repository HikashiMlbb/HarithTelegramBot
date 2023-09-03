using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Domain.Exceptions.Database;
using Microsoft.Extensions.Options;
using TelegramBot.Infrastructure.BotSettings;

namespace TelegramBot.Infrastructure;

public static class DependencyInjection
{
    private const string DefaultConnectionString = "SqliteDefault";
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, string? connectionString = null)
    {
        connectionString = config.GetConnectionString(connectionString ?? DefaultConnectionString);

        if (connectionString == null)
        {
            throw new ConnectionStringIsNotDefinedException(DefaultConnectionString);
        }

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
        
        AddCustomOptions(services);
        
        return services;
    }

    private static void AddCustomOptions(IServiceCollection services)
    {
        services.AddOptions<BotOptions>()
            .BindConfiguration(BotOptions.PathToSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}