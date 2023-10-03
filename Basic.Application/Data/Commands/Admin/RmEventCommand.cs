using Basic.Application.Data.Interfaces;
using Basic.Domain.Exceptions.Events;
using Basic.Domain.Interfaces;
using Partitions.Shared;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Basic.Application.Data.Commands.Admin;

[Command("rmevnt")]
public class RmEventCommand : ICommonCommand
{
    private readonly ITelegramBotClient _bot;
    private readonly IUnitOfWork _uow;
    private readonly ILogger _logger = Log.ForContext<RmEventCommand>();

    public RmEventCommand(IBotService bot, IUnitOfWork uow)
    {
        _uow = uow;
        _bot = bot.CurrentBot;
    }

    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        var userId = message.From!.Id;
        var chatId = message.Chat.Id;
        var chatMember = await _bot.GetChatMemberAsync(chatId, userId, cancellationToken);

        if (chatMember.Status != ChatMemberStatus.Creator)
        {
            return;
        }
        
        var arg = message.Text!.Split(
            ' ',
            2,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Last();
        
        arg = string.Concat(arg.Where(ch => ch != '\"'));

        try
        {
            var removedEvent = await _uow.Events.RemoveAsync(arg, chatId, cancellationToken);

            await _uow.CompleteAsync(cancellationToken);
            
            await _bot.SendTextMessageAsync(chatId, $"Событие \"{removedEvent.Name}\" было успешно удалено!",
                replyToMessageId: message.MessageId, cancellationToken: cancellationToken);

        }
        catch (EventNotFoundException)
        {
            await _bot.SendTextMessageAsync(chatId, $"Событие \"{arg}\" не найдено",
                replyToMessageId: message.MessageId, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error("Caught an exception RmEventCommand: {exception}", e.Message);
        }
    }
}