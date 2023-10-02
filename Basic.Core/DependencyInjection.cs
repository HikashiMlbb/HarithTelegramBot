using Basic.Application;
using Basic.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Basic.Core;

public static class DependencyInjection
{
    public static void AddBasicCore(this IServiceCollection services, HostBuilderContext hostCtx)
    {
        services.AddInfrastructure(hostCtx.Configuration, "Default");
        services.AddApplication();
    }
}