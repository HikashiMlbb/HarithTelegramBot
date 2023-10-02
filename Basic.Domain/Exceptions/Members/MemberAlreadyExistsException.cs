using Basic.Domain.Entities;

namespace Basic.Domain.Exceptions.Members;

[Serializable]
public sealed class MemberAlreadyExistsException : Exception
{
    public MemberAlreadyExistsException(Member member) : base(
        $"{member.FirstName} with data {{ tg://user?id={member.Account.TelegramId} and chat id ({member.Account.ChatId}) }} already exists in database")
    {
    }
}