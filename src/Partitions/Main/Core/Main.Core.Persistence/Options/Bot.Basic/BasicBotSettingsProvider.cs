using Main.Core.Application.Data.Interfaces;
using Microsoft.Extensions.Options;

namespace Main.Core.Persistence.Options.Bot.Basic;

public class BasicBotSettingsProvider : IBasicBotSettingsProvider
{
    private readonly IOptionsMonitor<BasicBotSettings> _optionsMonitor;

    public BasicBotSettingsProvider(IOptionsMonitor<BasicBotSettings> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    public TimeSpan? GetRewardInterval()
    {
        return _optionsMonitor.CurrentValue.RewardInterval;
    }

    public TimeSpan? GetMessageInterval()
    {
        return _optionsMonitor.CurrentValue.MessageInterval;
    }

    public Dictionary<string, float>? GetRewardSystem()
    {
        return _optionsMonitor.CurrentValue.RewardSystem;
    }
}