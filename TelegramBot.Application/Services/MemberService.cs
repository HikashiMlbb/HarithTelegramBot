using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Domain.Entities;

namespace TelegramBot.Application.Services;

public class MemberService : IMemberService
{
    public int GetNextLevel(BotMember member)
    {
        return member.Level + 1;
    }

    public float GetRequiredExperience(BotMember member)
    {
        var level = GetNextLevel(member);

        if (level == 2) return 15f;

        return level * 10f;
    }
}