using Telegram.Bot;

namespace Basic.Application.Data.Interfaces;

public interface IBot
{
    public ITelegramBotClient CurrentBot { get; }
    public Task StartAsync();
}