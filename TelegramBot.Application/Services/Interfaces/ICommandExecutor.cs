using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Commands.Common.AttributesAndInterfaces;

namespace TelegramBot.Application.Services.Interfaces;

public interface ICommandExecutor
{
    public Task<ICommonCommand?> FindCommandAsync(string commandName);
    public Task ExecuteCommandAsync(ICommonCommand command, Message message, CancellationToken cancellationToken);
}