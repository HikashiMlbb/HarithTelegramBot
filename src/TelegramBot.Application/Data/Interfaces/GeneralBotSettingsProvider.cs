namespace TelegramBot.Application.Data.Interfaces;

public interface IGeneralBotSettingsProvider
{
    public string? GetToken();
    public string? GetProxy();
}