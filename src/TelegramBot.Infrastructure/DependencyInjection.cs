using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Domain.Exceptions.Database;
using TelegramBot.Infrastructure.Data;

namespace TelegramBot.Infrastructure;

public static class DependencyInjection
{
    private const string DefaultConnectionString = "Default";

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config,
        string? connectionString = null)
    {
        connectionString = config.GetConnectionString(connectionString ?? DefaultConnectionString);

        if (connectionString == null) throw new ConnectionStringIsNotDefinedException(DefaultConnectionString);

        services.AddDbContext<BasicPartitionContext>(options =>
            {
                options.UseNpgsql(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure();
                });
            },
            ServiceLifetime.Singleton);
    }
}