using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Domain.Exceptions.Database;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Infrastructure.Data;
using TelegramBot.Infrastructure.Data.Options;
using TelegramBot.Infrastructure.Repositories;

namespace TelegramBot.Infrastructure;

public static class DependencyInjection
{
    private const string DefaultConnectionString = "Default";

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config,
        string? connectionString = null)
    {
        connectionString = config.GetConnectionString(connectionString ?? DefaultConnectionString);

        if (connectionString == null) throw new ConnectionStringIsNotDefinedException(DefaultConnectionString);

        services.AddDbContext<BasicPartitionContext>(options => { options.UseSqlServer(connectionString); },
            ServiceLifetime.Singleton);

        services.AddSingleton<IUnitOfWork, UnitOfWork>();

        AddCustomOptions(services);
    }

    private static void AddCustomOptions(IServiceCollection services)
    {
        services.AddOptions<BotSettings>()
            .BindConfiguration(BotSettings.PathToSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}