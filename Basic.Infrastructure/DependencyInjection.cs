using Basic.Infrastructure.Data;
using Basic.Infrastructure.Data.Options;
using Basic.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Basic.Domain.Exceptions.Database;
using Basic.Domain.Interfaces;

namespace Basic.Infrastructure;

public static class DependencyInjection
{
    private const string DefaultConnectionString = "Default";

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config,
        string? connectionString = null)
    {
        connectionString = config.GetConnectionString(connectionString ?? DefaultConnectionString);

        if (connectionString == null) throw new ConnectionStringIsNotDefinedException(DefaultConnectionString);

        services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connectionString); },
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