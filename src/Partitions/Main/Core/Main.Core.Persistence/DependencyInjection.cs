using Main.Core.Application.Data.Interfaces;
using Main.Core.Domain.Repositories;
using Main.Core.Persistence.Options.Bot.Basic;
using Main.Core.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Core.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWork, UnitOfWork>();

        services.AddOptions<BasicBotSettings>()
            .BindConfiguration(BasicBotSettings.PathToSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IBasicBotSettingsProvider, BasicBotSettingsProvider>();
    }
}