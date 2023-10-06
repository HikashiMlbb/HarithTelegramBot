using TelegramBot.Application.Data.Shared;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.ValueObjects;
using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Application.Services;
using TelegramBot.Domain.Repositories;

namespace TelegramBot.Application.Data.Handlers;

// ReSharper disable once UnusedType.Global
public class MessageHandler : IHandler
{
    private readonly IBotService _botService;
    private readonly CancellationToken _cancellationToken;
    private readonly ICommandExecutor _commandExecutor;
    private readonly IRewardService _rewardService;
    private readonly IUnitOfWork _uow;

    public MessageHandler(IBotService botService, ICommandExecutor commandExecutor, IRewardService rewardService, IUnitOfWork uow,
        IStoppingToken stoppingToken)
    {
        _botService = botService;
        _commandExecutor = commandExecutor;
        _rewardService = rewardService;
        _uow = uow;
        _cancellationToken = stoppingToken.Token;
    }

    public UpdateType UpdateType => UpdateType.Message;

    public async Task HandleAsync(Update update)
    {
        var message = update.Message!;
        // Checks if user has written a bot command at the begin of the message.
        if (message.IsCommand())
        {
            var textCommand = message.Text!.GetFirstCommand();

            if (await _commandExecutor.FindCommandAsync(textCommand) is not { } command) return;

            await command.ExecuteAsync(message, _cancellationToken);

            return;
        }

        #region Rewarding

        var telegramId = message.From!.Id;
        var chatId = message.Chat.Id;
        var account = new Account(telegramId, chatId);

        var hasLevelUpped = await RewardMember(account, message);

        var member = (await _uow.Members.FindUserByAccountAsync(account, _cancellationToken))!;

        if (hasLevelUpped) await CongratulateMember(member, chatId);

        #endregion
        
        await _uow.CompleteAsync(_cancellationToken);
    }

    private async Task CongratulateMember(Member member, long chatId)
    {
        var messageToCongratulate = $"""
                                     Поздравляем <a href="tg://user?id={member.Account.TelegramId}">{member.FirstName}</a> с
                                     достижением {member.Level}-го уровня! 🎉🎉🎉
                                     """;
        await _botService.CurrentBot.SendTextMessageAsync(chatId, messageToCongratulate,
            parseMode: ParseMode.Html,
            disableNotification: true,
            cancellationToken: _cancellationToken);
    }

    private async Task<bool> RewardMember(Account account, Message message)
    {
        var hasLevelUpped = await _rewardService.RewardAsync(account, message);
        return hasLevelUpped;
    }
}