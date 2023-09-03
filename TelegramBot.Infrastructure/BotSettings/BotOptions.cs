using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Infrastructure.BotSettings;

public sealed class BotOptions
{
    public const string PathToSection = nameof(BotOptions);
    
    [Required]
    public string Token { get; init; } = null!;
    
    [RegularExpression(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{4,5}")]
    public string? ProxyUri { get; init; } = null!;
}