using Main.Core.Domain.Exceptions.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Core.Infrastructure;

public static class DependencyInjection
{
    private const string DefaultConnectionString = "Default";

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config,
        string? connectionString = null)
    {
        connectionString = config.GetConnectionString(connectionString ?? DefaultConnectionString);

        if (connectionString == null) throw new ConnectionStringIsNotDefinedException(DefaultConnectionString);

        services.AddDbContext<BasicPartitionContext>(
            options => { options.UseNpgsql(connectionString, builder => { builder.EnableRetryOnFailure(); }); },
            ServiceLifetime.Singleton);
    }
}