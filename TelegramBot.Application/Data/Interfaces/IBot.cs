using Telegram.Bot;

namespace TelegramBot.Application.Data.Interfaces;

public interface IBot
{
    public ITelegramBotClient CurrentBot { get; }
    public Task StartAsync();
}