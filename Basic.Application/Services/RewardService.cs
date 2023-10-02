using Basic.Application.Data.Builders;
using Basic.Application.Data.Interfaces;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Basic.Domain.Interfaces;
using Basic.Domain.ValueObjects;
using Basic.Infrastructure.Data.Options;

namespace Basic.Application.Services;

public class RewardService : IRewardService
{
    private readonly IOptionsMonitor<BotSettings> _botOptions;
    private readonly IMemberService _memberService;
    private readonly IStoppingToken _stoppingToken;
    private readonly IUnitOfWork _uow;

    public RewardService(IOptionsMonitor<BotSettings> botOptions, IUnitOfWork uow, IStoppingToken stoppingToken,
        IMemberService memberService)
    {
        _botOptions = botOptions;
        _uow = uow;
        _stoppingToken = stoppingToken;
        _memberService = memberService;
    }

    public async Task<bool> RewardAsync(Account account, Message message)
    {
        var currentSettings = _botOptions.CurrentValue;
        var member = await _uow.Members.FindUserByAccountAsync(account, _stoppingToken.Token);

        if (member == null) return false;

        var hasLevelUpped =
            new RewardingBuilder(member, currentSettings, _memberService.GetRequiredExperience)
                .TryReward(message)
                .LevelUp()
                .UpdateLastRewardDate()
                .Build(out var currentLevel);

        return hasLevelUpped;
    }
}