using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Data.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Exceptions.Members;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Domain.ValueObjects;

namespace TelegramBot.Application.Data.Commands.Common.Basic;

[Command("start")]
public class StartCommand : ICommonCommand
{
    private readonly ITelegramBotClient _bot;
    private readonly IUnitOfWork _uow;

    public StartCommand(IUnitOfWork db, IBot bot)
    {
        _uow = db;
        _bot = bot.CurrentBot;
    }

    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        var telegramId = message.From!.Id;
        var chatId = message.Chat.Id;

        var account = new Account(telegramId, chatId);

        var member = new BotMember(message.From!.FirstName, account);

        try
        {
            await _uow.Members.AddAsync(member, cancellationToken);
            await _uow.CompleteAsync(cancellationToken);
            await _bot.SendTextMessageAsync(chatId, "You've registered!", cancellationToken: cancellationToken);
        }
        catch (MemberAlreadyExistsException)
        {
            var foundMember = (await _uow.Members.FindUserByAccountAsync(account, cancellationToken))!;
            var messageToSend = $"""
                                 You're already registered!
                                 Your level: {foundMember.Level}
                                 Your experience: {foundMember.Experience}
                                 """;
            await _bot.SendTextMessageAsync(chatId, messageToSend, protectContent: true,
                replyToMessageId: message.MessageId, cancellationToken: cancellationToken);
        }
    }
}