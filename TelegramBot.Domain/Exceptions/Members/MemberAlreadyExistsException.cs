using TelegramBot.Domain.Entities;

namespace TelegramBot.Domain.Exceptions.Members;

[Serializable]
public sealed class MemberAlreadyExistsException : Exception
{
    public MemberAlreadyExistsException(BotMember member) : base($"{member.FirstName} with data {{ tg://user?id={member.TelegramId} and chat id ({member.ChatId}) }} already exists in database")
    {
        
    }
}