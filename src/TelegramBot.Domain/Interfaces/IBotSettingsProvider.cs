namespace TelegramBot.Domain.Interfaces;

public interface IBotSettingsProvider
{
    public string? GetToken();
    public string? GetProxy();
    public TimeSpan? GetRewardInterval();
    public TimeSpan? GetMessageInterval();
    public Dictionary<string, float>? GetRewardSystem();
}