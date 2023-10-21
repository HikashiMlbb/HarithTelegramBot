using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Persistence.Options.Bot.General;

namespace TelegramBot.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddOptions<GeneralBotSettings>()
            .BindConfiguration(GeneralBotSettings.PathToSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IGeneralBotSettingsProvider, GeneralBotSettingsProvider>();
    }
}