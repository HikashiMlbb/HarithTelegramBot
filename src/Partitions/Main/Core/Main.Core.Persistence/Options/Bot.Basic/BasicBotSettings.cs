using System.ComponentModel.DataAnnotations;

namespace Main.Core.Persistence.Options.Bot.Basic;

public class BasicBotSettings
{
    public const string PathToSection = "BotSettings:Main";

    [Timestamp] public TimeSpan? RewardInterval { get; set; }

    [Timestamp] public TimeSpan? MessageInterval { get; set; }

    public Dictionary<string, float>? RewardSystem { get; set; }
}