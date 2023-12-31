﻿using System.Diagnostics.Contracts;
using Main.Core.Application.Data.Interfaces;
using Main.Core.Application.Services.Interfaces;
using Main.Core.Domain.Entities;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Main.Core.Application.Data.Builders;

internal class RewardingBuilder : IRewardingBuilder
{
    private readonly Func<Member, float> _getRequiredExperience;
    private readonly Member _member;
    private readonly IBasicBotSettingsProvider _settings;

    private bool _hasLevelUpped;
    private bool _isRewarded;
    private bool _shouldUpdateDate;

    public RewardingBuilder(Member member,
        IBasicBotSettingsProvider settings,
        Func<Member, float> getRequiredExperience)
    {
        _member = member;
        _settings = settings;
        _getRequiredExperience = getRequiredExperience;
    }

    public IRewardingBuilder TryReward(Message message)
    {
        var interval = DateTime.UtcNow - _member.LastMessageAt;
        if (interval <= _settings.GetMessageInterval()) return this;

        float experienceToAdd = default;
        _settings.GetRewardSystem()?.TryGetValue(message.Type.ToString(), out experienceToAdd);

        experienceToAdd = ComputeExperienceToAdd(message, experienceToAdd);
        _member.ExperienceToReward += experienceToAdd;
        _isRewarded = experienceToAdd > default(float);

        return this;
    }

    public IRewardingBuilder TryReward(Message message, IEnumerable<Event> events)
    {
        var interval = DateTime.UtcNow - _member.LastMessageAt;
        if (interval <= _settings.GetMessageInterval()) return this;

        float experienceToAdd = default;
        _settings.GetRewardSystem()?.TryGetValue(message.Type.ToString(), out experienceToAdd);

        experienceToAdd = ComputeExperienceToAdd(message, experienceToAdd);
        experienceToAdd = ComputeWithEvents(experienceToAdd, events);
        _member.ExperienceToReward += experienceToAdd;
        _isRewarded = experienceToAdd > default(float);

        return this;
    }

    public IRewardingBuilder LevelUp()
    {
        var interval = DateTime.UtcNow - _member.LastRewardAt;

        if (_isRewarded == false || interval <= _settings.GetRewardInterval()) return this;

        _shouldUpdateDate = true;
        _member.Experience += _member.ExperienceToReward;
        _member.ExperienceToReward = default;

        _hasLevelUpped = LevelUpRecursively();

        return this;
    }

    public IRewardingBuilder UpdateLastRewardDate()
    {
        if (_isRewarded) _member.LastMessageAt = DateTime.UtcNow;
        if (!_isRewarded || !_shouldUpdateDate) return this;

        _member.LastRewardAt = DateTime.UtcNow;

        return this;
    }

    /// <summary>
    ///     Build the builder
    /// </summary>
    /// <returns>True, if level has been upped. Else - false</returns>
    public bool Build()
    {
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

    [Pure]
    private float ComputeWithEvents(float experienceToAdd, IEnumerable<Event> events)
    {
        var totalMultiplier = events.Select(@event => @event.Multiplier).Aggregate(1f, (acc, x) => acc * x);
        return experienceToAdd * totalMultiplier;
    }

    #endregion
}