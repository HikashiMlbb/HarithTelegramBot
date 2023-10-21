using Telegram.Bot.Types;
using TelegramBot.Partitions.Shared.Commands;

namespace TelegramBot.Application.Services.Interfaces;

public interface ICommandExecuteService
{
    public Task<ITextCommand?> FindCommandAsync(string commandName);
    public Task ExecuteCommandAsync(ITextCommand commandHandler, Message message, CancellationToken cancellationToken);
}