using TelegramBot.Domain.Entities;

namespace TelegramBot.Application.Data.Interfaces;

public interface IMemberService
{
    public int GetNextLevel(BotMember member);
    public float GetRequiredExperience(BotMember member);
}