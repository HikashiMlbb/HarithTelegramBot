namespace Main.Core.Application.Data.Interfaces;

public interface IBasicBotSettingsProvider
{
    public TimeSpan? GetRewardInterval();
    public TimeSpan? GetMessageInterval();
    public Dictionary<string, float>? GetRewardSystem();
}