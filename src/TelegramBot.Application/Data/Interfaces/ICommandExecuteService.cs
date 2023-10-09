using Telegram.Bot.Types;
using TelegramBot.Application.Data.Shared;

namespace TelegramBot.Application.Data.Interfaces;

public interface ICommandExecuteService
{
    public Task<ITextCommand?> FindCommandAsync(string commandName);
    public Task ExecuteCommandAsync(ITextCommand commandHandler, Message message, CancellationToken cancellationToken);
}