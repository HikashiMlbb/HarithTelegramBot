using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Exceptions.Members;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Partitions.Shared;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Application.Data.Commands;

[Command("start")]
public class StartCommand : ICommonCommand
{
    private readonly ITelegramBotClient _bot;
    private readonly IMemberService _memberService;
    private readonly IUnitOfWork _uow;
    private readonly IBotService _ibot;

    public StartCommand(IUnitOfWork db, IBotService botService, IMemberService memberService)
    {
        _uow = db;
        _memberService = memberService;
        _ibot = botService;
        _bot = botService.CurrentBot;
    }

    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        var telegramId = message.From!.Id;
        var chatId = message.Chat.Id;

        var account = new Account(telegramId, chatId);

        var member = new Member(message.From!.FirstName, account);
        ICommonCommand command = new StatCommand(_uow, _memberService, _ibot);

        try
        {
            await _uow.Members.AddAsync(member, cancellationToken);
            await _uow.CompleteAsync(cancellationToken);
            
            var foundMember = (await _uow.Members.FindUserByAccountAsync(account, cancellationToken))!;

            string stat = "Вы успешно зарегистрировались!\n" + _memberService.GetStat(foundMember);

            await _bot.SendTextMessageAsync(chatId, stat, replyToMessageId: message.MessageId, cancellationToken: cancellationToken);
        }
        catch (MemberAlreadyExistsException)
        {
            var foundMember = (await _uow.Members.FindUserByAccountAsync(account, cancellationToken))!;
            string stat = _memberService.GetStat(foundMember);
            
            await _bot.SendTextMessageAsync(chatId, stat, replyToMessageId: message.MessageId, cancellationToken: cancellationToken);
        }
    }
}

/* TODO:
 * - Create StatCommand to check out stats
 * - Make StartCommand get depend on StatCommand and call it when member has already registered himself.
 */