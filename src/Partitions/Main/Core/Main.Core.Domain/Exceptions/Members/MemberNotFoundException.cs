using Main.Core.Domain.Entities;

namespace Main.Core.Domain.Exceptions.Members;

[Serializable]
public class MemberNotFoundException : Exception
{
    public MemberNotFoundException(Member member) : base(
        $"{member.FirstName} with data {{ {member.Account} }} is not found. It is not possible to change object values")
    {
    }
}