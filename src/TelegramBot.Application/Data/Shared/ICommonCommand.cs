using Telegram.Bot.Types;

namespace TelegramBot.Application.Data.Shared;

public interface ICommonCommand
{
    public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
}