using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Domain.Repositories;
using TelegramBot.Persistence.Options.Bot;
using TelegramBot.Persistence.Repositories;

namespace TelegramBot.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWork, UnitOfWork>();

        AddOptions(services);
    }
    private static void AddOptions(IServiceCollection services)
    {
        services.AddOptions<BotSettings>()
            .BindConfiguration(BotSettings.PathToSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton<IBotSettingsProvider, BotSettingsProvider>();
    }
}