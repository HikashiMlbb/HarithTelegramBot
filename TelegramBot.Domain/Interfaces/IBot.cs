using Telegram.Bot;

namespace TelegramBot.Domain.Interfaces;

public interface IBot
{
    public ITelegramBotClient CurrentBot { get; }
}