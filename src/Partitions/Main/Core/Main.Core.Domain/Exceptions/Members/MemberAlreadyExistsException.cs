using Main.Core.Domain.Entities;

namespace Main.Core.Domain.Exceptions.Members;

[Serializable]
public sealed class MemberAlreadyExistsException : Exception
{
    public MemberAlreadyExistsException(Member member) : base(
        $"{member.FirstName} with data {{ tg://user?id={member.Account.ChatId} and chat id ({member.Account.ChatId}) }} already exists in database")
    {
    }
}