using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Application.Commands.Common.AttributesAndInterfaces;

public interface ICommonCommand
{
    public Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken cancellationToken);
}