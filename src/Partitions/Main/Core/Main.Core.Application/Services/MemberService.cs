using Main.Core.Application.Services.Interfaces;
using Main.Core.Domain.Entities;

namespace Main.Core.Application.Services;

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
        var currentExp = member.Experience;
        var requiredExp = GetRequiredExperience(member);
        var filledSquaresCount = (int)(currentExp * 10 / requiredExp);

        var progressBar = new string('\u25a1', 10).ToCharArray();
        ReplaceFirst(progressBar, filledSquaresCount);

        var stat = $"""
                    Уровень: {member.Level}
                    Опыт: {currentExp:F}/{requiredExp}
                    [{string.Join(' ', progressBar)}]
                    """;
        return stat;
    }

    private static void ReplaceFirst(char[] array, int count)
    {
        for (var i = 0; i < array.Length; i++)
        {
            if (i >= count) continue;
            array[i] = '\u25a3';
        }
    }
}