using Microsoft.Extensions.Options;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Persistence.Options.Bot.General;

public class GeneralBotSettingsProvider : IGeneralBotSettingsProvider
{
    private readonly IOptionsMonitor<GeneralBotSettings> _botSettingsMonitor;

    public GeneralBotSettingsProvider(IOptionsMonitor<GeneralBotSettings> botSettingsMonitor)
    {
        _botSettingsMonitor = botSettingsMonitor;
    }

    public string? GetToken()
    {
        return _botSettingsMonitor.CurrentValue.Token;
    }

    public string? GetProxy()
    {
        return _botSettingsMonitor.CurrentValue.Proxy;
    }
}