using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Services.Interfaces;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Exceptions.Members;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Infrastructure.Data;

namespace TelegramBot.Application.Commands.Common;

[Command("addme")]
public class AddMemberCommand : ICommonCommand
{
    private readonly IBotMembersRepository _botMembersRepository;
    private readonly ApplicationDbContext _db;
    private readonly ITelegramBotClient _bot;

    public AddMemberCommand(IBotMembersRepository botMembersRepository, ApplicationDbContext db, IBot bot)
    {
        _botMembersRepository = botMembersRepository;
        _db = db;
        _bot = bot.CurrentBot;
    }
    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        long telegramId = message.From!.Id;
        long chatId = message.Chat.Id;

        BotMember memberToAdd = new BotMember(
            new Guid(),
            message.From!.FirstName,
            telegramId,
            chatId);

        try
        {
            await _botMembersRepository.AddAsync(memberToAdd, cancellationToken);
            int count = await _db.SaveChangesAsync(cancellationToken);

            await _bot.SendTextMessageAsync(chatId, $"Changed: {count}", cancellationToken: cancellationToken);
        }
        catch (MemberAlreadyExistsException e)
        {
            await _bot.SendTextMessageAsync(chatId, $"Oops, you are already in database", cancellationToken: cancellationToken);
        }
        
    }
}