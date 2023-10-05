using Telegram.Bot;

namespace TelegramBot.Application.Data.Interfaces;

public interface IBotService
{
    public ITelegramBotClient CurrentBot { get; }
    public Task StartAsync();
}