using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Application.Data.Shared;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Exceptions.Members;
using TelegramBot.Domain.Repositories;
using TelegramBot.Domain.ValueObjects;

namespace TelegramBot.Application.Data.Commands.Basic.Start;

[Command("start")]
public class StartCommandHandler : ITextCommandHandler<StartCommand>
{
    private readonly ITelegramBotClient _bot;
    private readonly IMemberService _memberService;
    private readonly IUnitOfWork _uow;

    public StartCommandHandler(IUnitOfWork db, IBotService botService, IMemberService memberService)
    {
        _uow = db;
        _memberService = memberService;
        _bot = botService.CurrentBot;
    }

    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        var telegramId = message.From!.Id;
        var chatId = message.Chat.Id;

        var account = new Account(telegramId, chatId);

        var member = new Member(message.From!.FirstName, account);

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