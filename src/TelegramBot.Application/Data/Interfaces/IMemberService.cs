using TelegramBot.Domain.Entities;

namespace TelegramBot.Application.Data.Interfaces;

public interface IMemberService
{
    public int GetNextLevel(Member member);
    public float GetRequiredExperience(Member member);
    public string GetStat(Member member);
}