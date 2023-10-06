using Telegram.Bot.Types;
using TelegramBot.Domain.ValueObjects;
using TelegramBot.Application.Data.Builders;
using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Domain.Repositories;

namespace TelegramBot.Application.Services;

public class RewardService : IRewardService
{
    private readonly IBotSettingsProvider _botSettingsProvider;
    private readonly IMemberService _memberService;
    private readonly IStoppingToken _stoppingToken;
    private readonly IUnitOfWork _uow;

    public RewardService(IBotSettingsProvider botSettingsProvider, IUnitOfWork uow, IStoppingToken stoppingToken,
        IMemberService memberService)
    {
        _botSettingsProvider = botSettingsProvider;
        _uow = uow;
        _stoppingToken = stoppingToken;
        _memberService = memberService;
    }

    public async Task<bool> RewardAsync(Account account, Message message)
    {
        var member = await _uow.Members.FindUserByAccountAsync(account, _stoppingToken.Token);

        if (member == null) return false;

        IRewardingBuilder rewardingBuilder =
            new RewardingBuilder(member, _botSettingsProvider, _memberService.GetRequiredExperience);
        
        var hasLevelUpped = rewardingBuilder
                .TryReward(message)
                .LevelUp()
                .UpdateLastRewardDate()
                .Build();

        return hasLevelUpped;
    }
}