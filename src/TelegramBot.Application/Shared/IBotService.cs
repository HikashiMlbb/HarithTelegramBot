using Telegram.Bot;

namespace TelegramBot.Application.Shared;

public interface IBotService
{
    public ITelegramBotClient CurrentBot { get; }
    public Task StartAsync();
}