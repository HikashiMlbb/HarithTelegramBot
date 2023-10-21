namespace TelegramBot.Persistence.Options.Bot.General;

public sealed class GeneralBotSettings
{
    public const string PathToSection = "BotSettings:General";

    public string? Token { get; set; } = null!;
    public string? Proxy { get; set; }
}