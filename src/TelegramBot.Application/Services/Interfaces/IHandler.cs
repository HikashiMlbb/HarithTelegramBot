using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Application.Services.Interfaces;

public interface IHandler
{
    public UpdateType UpdateType { get; }
    public Task HandleAsync(Update update);
}