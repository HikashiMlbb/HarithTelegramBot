using Telegram.Bot.Types;
using TelegramBot.Application.Data.Shared;

namespace TelegramBot.Application.Data.Interfaces;

public interface ICommandExecutor
{
    public Task<ICommonCommand?> FindCommandAsync(string commandName);
    public Task ExecuteCommandAsync(ICommonCommand command, Message message, CancellationToken cancellationToken);
}