using Telegram.Bot.Types;

namespace TelegramBot.Application.Shared;

public interface IMessageHandler
{
    public Task Handle(Update update, CancellationToken cancellationToken = default);
}