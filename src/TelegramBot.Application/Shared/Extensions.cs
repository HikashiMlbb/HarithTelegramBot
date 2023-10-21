using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Application.Shared;

public static class Extensions
{
    public static string GetFirstCommand(this string str)
    {
        return string.Concat(str.Skip(1).TakeWhile(ch => !char.IsWhiteSpace(ch)));
    }

    public static bool IsCommand(this Message message)
    {
        return message.Entities is { } entities &&
               entities.Any(entity => entity is { Type: MessageEntityType.BotCommand, Offset: 0 });
    }
}