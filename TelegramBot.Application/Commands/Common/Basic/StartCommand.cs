using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Services.Interfaces;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Exceptions.Members;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Domain.ValueObjects;
using TelegramBot.Infrastructure.Data;

namespace TelegramBot.Application.Commands.Common.Basic;

[Command("start")]
public class StartCommand : ICommonCommand
{
    private readonly ApplicationDbContext _db;
    private readonly IBotMembersRepository _membersRepository;
    private readonly ITelegramBotClient _bot;
    
    public StartCommand(ApplicationDbContext db, IBotMembersRepository membersRepository, IBot bot)
    {
        _db = db;
        _membersRepository = membersRepository;
        _bot = bot.CurrentBot;
    }
    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        long telegramId = message.From!.Id;
        long chatId = message.Chat.Id;
        
        Account account = new Account(telegramId, chatId);
        
        BotMember member = new BotMember(message.From!.FirstName, account);

        try
        {
            await _membersRepository.AddAsync(member, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            await _bot.SendTextMessageAsync(chatId, "You've registered!", cancellationToken: cancellationToken);
        }
        catch (MemberAlreadyExistsException e)
        {
            BotMember foundMember = (await _membersRepository.FindUserByAccountAsync(account, cancellationToken))!;
            string messageToSend = $"""
                             You're already registered!
                             Your level: {foundMember.Level}
                             Your experience: {foundMember.Experience}
                             """;
            await _bot.SendTextMessageAsync(chatId, messageToSend, protectContent: true,
                replyToMessageId: message.MessageId, cancellationToken: cancellationToken);
        }
        
    }
}