using TelegramBot.Domain.Entities;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Application.Services;

public class MemberService : IMemberService
{
    public int GetNextLevel(Member member)
    {
        return member.Level + 1;
    }

    public float GetRequiredExperience(Member member)
    {
        var level = GetNextLevel(member);

        if (level == 2) return 15f;

        return level * 10f;
    }

    public string GetStat(Member member)
    {
        float currentExp = member.Experience;
        float requiredExp = GetRequiredExperience(member);
        int filledSquaresCount = (int)(currentExp * 10 / requiredExp);

        char[] progressBar = new string('\u25a1', 10).ToCharArray();
        ReplaceFirst(progressBar, (int)(currentExp  / (requiredExp / 10)));
        
        string stat = $"""
                       Уровень: {member.Level}
                       Опыт: {currentExp:F}/{requiredExp}
                       [{string.Join(' ', progressBar)}]
                       """;
        return stat;
    }

    private void ReplaceFirst(char[] array, int count)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (i >= count) continue;
            array[i] = '\u25a3';
        }
    }
}