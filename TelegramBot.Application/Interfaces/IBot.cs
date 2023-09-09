using Telegram.Bot;

namespace TelegramBot.Application.Interfaces;

public interface IBot
{
    public ITelegramBotClient CurrentBot { get; }
}