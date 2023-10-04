using Telegram.Bot.Types;

namespace Partitions.Shared;

public interface ICommonCommand
{
    public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
}