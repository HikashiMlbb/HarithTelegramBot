using Basic.Application.Data.Interfaces;
using Basic.Domain.Entities;
using Basic.Domain.Interfaces;
using Basic.Domain.ValueObjects;
using Partitions.Shared;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Basic.Application.Data.Commands;

[Command("stat")]
public class StatCommand : ICommonCommand
{
    private readonly IUnitOfWork _uow;
    private readonly IMemberService _memberService;
    private readonly ITelegramBotClient _bot;
    
    public StatCommand(IUnitOfWork uow, IMemberService memberService, IBot bot)
    {
        _uow = uow;
        _memberService = memberService;
        _bot = bot.CurrentBot;
    }
    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        var telegramId = message.From!.Id;
        var chatId = message.Chat.Id;

        var account = new Account(telegramId, chatId);

        Member? member = await _uow.Members.FindUserByAccountAsync(account, cancellationToken);

        if (member is null)
        {
            return;
        }

        string messageToSend = _memberService.GetStat(member);
        
        await _bot.SendTextMessageAsync(chatId, messageToSend,
            replyToMessageId: message.MessageId, cancellationToken: cancellationToken);
    }
}