using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Domain.ValueObjects;
using TelegramBot.Infrastructure.Data.Options;
using TelegramBot.Application.Data.Builders;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Application.Services;

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