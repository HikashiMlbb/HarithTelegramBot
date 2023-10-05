using Microsoft.Extensions.Options;
using TelegramBot.Domain.Interfaces;

namespace TelegramBot.Persistence.Options.Bot;

public class BotSettingsProvider : IBotSettingsProvider
{
    private readonly IOptionsMonitor<BotSettings> _botSettingsMonitor;

    public BotSettingsProvider(IOptionsMonitor<BotSettings> botSettingsMonitor)
    {
        _botSettingsMonitor = botSettingsMonitor;
    }

    public string? GetToken() => _botSettingsMonitor.CurrentValue.Token;
    public string? GetProxy() => _botSettingsMonitor.CurrentValue.Proxy;
    public TimeSpan? GetRewardInterval() => _botSettingsMonitor.CurrentValue.RewardInterval;
    public TimeSpan? GetMessageInterval() => _botSettingsMonitor.CurrentValue.MessageInterval;
    public Dictionary<string, float>? GetRewardSystem() => _botSettingsMonitor.CurrentValue.RewardSystem;
}