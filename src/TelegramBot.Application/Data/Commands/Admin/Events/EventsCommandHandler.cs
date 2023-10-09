using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Application.Data.Shared;
using TelegramBot.Domain.Repositories;

namespace TelegramBot.Application.Data.Commands.Admin.Events;

[Command("events")]
public class EventsCommandHandler : ITextCommandHandler<EventsCommand>
{
    private readonly ITelegramBotClient _bot;
    private readonly IUnitOfWork _uow;

    public EventsCommandHandler(IBotService bot, IUnitOfWork uow)
    {
        _bot = bot.CurrentBot;
        _uow = uow;
    }

    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        var userId = message.From!.Id;
        var chatId = message.Chat.Id;
        var result = await _bot.GetChatMemberAsync(chatId, userId, cancellationToken);

        if (result.Status is not ChatMemberStatus.Creator)
        {
            return;
        }

        var events = (await _uow.Events.AllAsync(chatId, cancellationToken)).ToList();

        if (events.Count == 0)
        {
            await _bot.SendTextMessageAsync(chatId, "В этом чате никаких событий нет :)", cancellationToken: cancellationToken);
            return;
        }

        var messageToSend = string.Join('\n', events.Select((@event, index) => $"{index + 1}. {@event.Name} -- x{@event.Multiplier:F}"));
        var totalMultiplier = events.Select(x => x.Multiplier).Aggregate(1f, (acc, x) => acc * x);
        messageToSend += $"\nИтого: x{totalMultiplier:F}";

        await _bot.SendTextMessageAsync(chatId, messageToSend, replyToMessageId: message.MessageId, cancellationToken: cancellationToken);
    }
}