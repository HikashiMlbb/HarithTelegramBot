using Main.Core.Application.Services.Interfaces;
using Main.Core.Domain.Repositories;
using Main.Core.Domain.ValueObjects;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Shared;
using TelegramBot.Partitions.Shared.Commands;

namespace Main.Core.Application.Data.Commands.Public.Stat;

[Command("stat")]
public class StatCommandHandler : ITextCommandHandler<StatCommand>
{
    private readonly ITelegramBotClient _bot;
    private readonly IMemberService _memberService;
    private readonly IUnitOfWork _uow;

    public StatCommandHandler(IUnitOfWork uow, IMemberService memberService, IBotService botService)
    {
        _uow = uow;
        _memberService = memberService;
        _bot = botService.CurrentBot;
    }

    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        var telegramId = message.From!.Id;
        var chatId = message.Chat.Id;

        var account = new Account(telegramId, chatId);

        var member = await _uow.Members.FindUserByAccountAsync(account, cancellationToken);

        if (member is null) return;

        var messageToSend = _memberService.GetStat(member);

        await _bot.SendTextMessageAsync(chatId, messageToSend,
            replyToMessageId: message.MessageId, cancellationToken: cancellationToken);
    }
}