using Telegram.Bot;

namespace Basic.Application.Data.Interfaces;

public interface IBotService
{
    public ITelegramBotClient CurrentBot { get; }
    public Task StartAsync();
}