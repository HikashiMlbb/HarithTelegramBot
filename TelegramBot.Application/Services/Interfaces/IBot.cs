using Telegram.Bot;

namespace TelegramBot.Application.Services.Interfaces;

public interface IBot
{
    public ITelegramBotClient CurrentBot { get; }
}