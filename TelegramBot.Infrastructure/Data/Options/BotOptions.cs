using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Infrastructure.Data.Options;

public sealed class BotOptions
{
    public const string PathToSection = nameof(BotOptions);
    
    public string? Token { get; init; } = null!;
    public string? Proxy { get; init; }
}