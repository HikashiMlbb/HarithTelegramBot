﻿using TelegramBot.Domain.Entities;

namespace TelegramBot.Domain.Exceptions.Members;

[Serializable]
public class MemberNotFoundException : Exception
{
    public MemberNotFoundException(BotMember member) : base($"{member.FirstName} with data {{ {member.TelegramId}, {member.ChatId} }} is not found. It is not possible to change object values")
    {
        
    }
}