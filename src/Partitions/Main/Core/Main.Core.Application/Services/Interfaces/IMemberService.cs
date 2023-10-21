using Main.Core.Domain.Entities;

namespace Main.Core.Application.Services.Interfaces;

public interface IMemberService
{
    public int GetNextLevel(Member member);
    public float GetRequiredExperience(Member member);
    public string GetStat(Member member);
}