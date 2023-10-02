using Basic.Domain.Entities;

namespace Basic.Application.Data.Interfaces;

public interface IMemberService
{
    public int GetNextLevel(Member member);
    public float GetRequiredExperience(Member member);
    public string GetStat(Member member);
}