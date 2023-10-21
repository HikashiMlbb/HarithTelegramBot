using Main.Core.Application.Services.Interfaces;
using Main.Core.Domain.Entities;
using Main.Core.Domain.Exceptions.Members;
using Main.Core.Domain.Repositories;
using Main.Core.Domain.ValueObjects;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Shared;
using TelegramBot.Partitions.Shared.Commands;

namespace Main.Core.Application.Data.Commands.Public.Start;

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
