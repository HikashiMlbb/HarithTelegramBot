using Main.Core.Application;
using Main.Core.Infrastructure;
using Main.Core.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Core;

public static class DependencyInjection
{
    public static void AddBasicPartition(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration, "PostgreSQL");
        services.AddPersistence();
    }
}