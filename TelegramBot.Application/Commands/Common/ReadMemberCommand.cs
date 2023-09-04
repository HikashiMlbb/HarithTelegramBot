using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Services.Interfaces;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Infrastructure.Data;

namespace TelegramBot.Application.Commands.Common;

[Command("readme")]
public class ReadMemberCommand : ICommonCommand
{
    private readonly IBotMembersRepository _botMembersRepository;
    private readonly ApplicationDbContext _db;
    private readonly ITelegramBotClient _bot;

    public ReadMemberCommand(IBotMembersRepository botMembersRepository, ApplicationDbContext db, IBot bot)
    {
        _botMembersRepository = botMembersRepository;
        _db = db;
        _bot = bot.CurrentBot;
    }
    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        long telegramId = message.From!.Id;
        long chatId = message.Chat.Id;

        BotMember? member = await _botMembersRepository.FindUserByTelegramAndChatIdAsync(telegramId, chatId, cancellationToken);

        if (member is null)
        {
            await _bot.SendTextMessageAsync(chatId, $"You have not found :(", cancellationToken: cancellationToken);
            return;
        }

        await _bot.SendTextMessageAsync(chatId, $"Yes, you in database", cancellationToken: cancellationToken);
    }
}