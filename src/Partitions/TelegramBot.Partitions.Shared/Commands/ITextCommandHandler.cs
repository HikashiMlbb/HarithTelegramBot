using Telegram.Bot.Types;

namespace TelegramBot.Partitions.Shared.Commands;

public interface ITextCommandHandler<T> where T : ITextCommand
{
    public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
}