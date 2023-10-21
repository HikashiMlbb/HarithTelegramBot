using Main.Core.Application.Services.Interfaces;
using Main.Core.Domain.Entities;
using Main.Core.Domain.Repositories;
using Main.Core.Domain.ValueObjects;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Shared;

namespace Main.Core.Application.Data.Handlers;

public class MessageHandler : IMessageHandler
{
    private readonly IUnitOfWork _uow;
    private readonly CancellationToken _cancellationToken;
    private readonly IRewardService _rewardService;
    private readonly ITelegramBotClient _botService;

    public MessageHandler(IUnitOfWork uow, IStoppingToken stoppingToken, IRewardService rewardService, IBotService botService)
    {
        _uow = uow;
        _rewardService = rewardService;
        _cancellationToken = stoppingToken.Token;
        _botService = botService.CurrentBot;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        var message = update.Message!;
        
        if (message.IsCommand())
        {
            return;
        }

        #region TrynaToReward
        var telegramId = message.From!.Id;
        var chatId = message.Chat.Id;
        var account = new Account(telegramId, chatId);

        var hasLevelUpped = await RewardMember(account, message);

        var member = (await _uow.Members.FindUserByAccountAsync(account, _cancellationToken))!;

        if (hasLevelUpped) await CongratulateMember(member, chatId);
        #endregion
        
        await _uow.CompleteAsync(_cancellationToken);
    }
    
    private async Task<bool> RewardMember(Account account, Message message)
    {
        var hasLevelUpped = await _rewardService.RewardAsync(account, message);
        return hasLevelUpped;
    }

    private async Task CongratulateMember(Member member, long chatId)
    {
        var messageToCongratulate = $"""
                                     Поздравляем <a href="tg://user?id={member.Account.TelegramId}">{member.FirstName}</a> с
                                     достижением {member.Level}-го уровня! 🎉🎉🎉
                                     """;
        await _botService.SendTextMessageAsync(chatId, messageToCongratulate,
            parseMode: ParseMode.Html,
            disableNotification: true,
            cancellationToken: _cancellationToken);
    }
}