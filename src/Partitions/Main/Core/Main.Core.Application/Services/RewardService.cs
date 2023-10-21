using Main.Core.Application.Data.Builders;
using Main.Core.Application.Data.Interfaces;
using Main.Core.Application.Services.Interfaces;
using Main.Core.Domain.Repositories;
using Main.Core.Domain.ValueObjects;
using Telegram.Bot.Types;
using TelegramBot.Application.Shared;

namespace Main.Core.Application.Services;

public class RewardService : IRewardService
{
    private readonly IBasicBotSettingsProvider _basicBotSettingsProvider;
    private readonly IMemberService _memberService;
    private readonly IStoppingToken _stoppingToken;
    private readonly IUnitOfWork _uow;

    public RewardService(IBasicBotSettingsProvider basicBotSettingsProvider, IUnitOfWork uow,
        IStoppingToken stoppingToken,
        IMemberService memberService)
    {
        _basicBotSettingsProvider = basicBotSettingsProvider;
        _uow = uow;
        _stoppingToken = stoppingToken;
        _memberService = memberService;
    }

    public async Task<bool> RewardAsync(Account account, Message message)
    {
        var member = await _uow.Members.FindUserByAccountAsync(account, _stoppingToken.Token);

        if (member == null) return false;

        IRewardingBuilder rewardingBuilder =
            new RewardingBuilder(member, _basicBotSettingsProvider, _memberService.GetRequiredExperience);
        var events = await _uow.Events.AllAsync(account.ChatId, _stoppingToken.Token);

        var hasLevelUpped = rewardingBuilder
            .TryReward(message, events)
            .LevelUp()
            .UpdateLastRewardDate()
            .Build();

        return hasLevelUpped;
    }
}