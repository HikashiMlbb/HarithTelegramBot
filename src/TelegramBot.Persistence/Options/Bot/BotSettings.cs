using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Persistence.Options.Bot;

public sealed class BotSettings
{
    public const string PathToSection = nameof(BotSettings);

    public string? Token { get; set; } = null!;
    public string? Proxy { get; set; }
    [Timestamp] public TimeSpan? RewardInterval { get; set; }
    [Timestamp] public TimeSpan? MessageInterval { get; set; }
    public Dictionary<string, float>? RewardSystem { get; set; }
}