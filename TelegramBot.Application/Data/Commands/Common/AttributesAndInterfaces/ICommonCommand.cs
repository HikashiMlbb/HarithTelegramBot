using Telegram.Bot.Types;

namespace TelegramBot.Application.Data.Commands.Common.AttributesAndInterfaces;

public interface ICommonCommand
{
    public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
}