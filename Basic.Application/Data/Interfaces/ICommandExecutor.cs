using Partitions.Shared;
using Telegram.Bot.Types;

namespace Basic.Application.Data.Interfaces;

public interface ICommandExecutor
{
    public Task<ICommonCommand?> FindCommandAsync(string commandName);
    public Task ExecuteCommandAsync(ICommonCommand command, Message message, CancellationToken cancellationToken);
}