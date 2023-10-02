using Basic.Application.Data.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Basic.Domain.Entities;
using Basic.Infrastructure.Data.Options;

namespace Basic.Application.Data.Builders;

internal class RewardingBuilder : IRewardingBuilder
{
    private readonly Func<Member, float> _getRequiredExperience;
    private readonly Member _member;
    private readonly BotSettings _settings;
    private bool _hasLevelUpped;

    private bool _isRewarded;
    private bool _shouldUpdateDate = true;

    public RewardingBuilder(Member member,
        BotSettings settings,
        Func<Member, float> getRequiredExperience)
    {
        _member = member;
        _settings = settings;
        _getRequiredExperience = getRequiredExperience;
    }

    public IRewardingBuilder TryReward(Message message)
    {
        var interval = DateTime.UtcNow - _member.LastMessageAt;
        if (interval <= _settings.MessageInterval) return this;

        float experienceToAdd = default;
        _settings.RewardSystem?.TryGetValue(message.Type.ToString(), out experienceToAdd);

        experienceToAdd = ComputeExperienceToAdd(message, experienceToAdd);
        _member.ExperienceToReward += experienceToAdd;
        _isRewarded = experienceToAdd > default(float);

        return this;
    }

    public IRewardingBuilder LevelUp()
    {
        var interval = DateTime.UtcNow - _member.LastRewardAt;

        if (_isRewarded == false || interval <= _settings.RewardInterval)
        {
            _shouldUpdateDate = false;
            return this;
        }

        _member.Experience += _member.ExperienceToReward;
        _member.ExperienceToReward = default;

        _hasLevelUpped = LevelUpRecursively();

        return this;
    }

    public IRewardingBuilder UpdateLastRewardDate()
    {
        if (_isRewarded == false || !_shouldUpdateDate) return this;

        _member.LastRewardAt = DateTime.UtcNow;

        return this;
    }

    /// <summary>
    ///     Build the builder
    /// </summary>
    /// <param name="currentLevel">level which is current</param>
    /// <returns>True, if level has been upped. Else - false</returns>
    public bool Build(out int currentLevel)
    {
        currentLevel = _member.Level;
        return _hasLevelUpped;
    }

    #region Private methods

    private static float ComputeExperienceToAdd(Message message, float experienceToAdd)
    {
        experienceToAdd = message.Type switch
        {
            MessageType.Text => experienceToAdd * GetRewardableCharactersCount(message.Text!),
            MessageType.Voice => experienceToAdd * message.Voice!.Duration,
            MessageType.Video => experienceToAdd * message.Video!.Duration,
            MessageType.VideoNote => experienceToAdd * message.VideoNote!.Duration,
            _ => experienceToAdd
        };
        return experienceToAdd;

        int GetRewardableCharactersCount(string str)
        {
            return str.Count(char.IsLetter);
        }
    }

    private bool LevelUpRecursively()
    {
        var hasLevelUpped = false;
        while (_member.Experience >= _getRequiredExperience(_member))
        {
            _member.Experience -= _getRequiredExperience(_member);
            _member.Level += 1;
            hasLevelUpped = true;
        }

        return hasLevelUpped;
    }

    #endregion
}