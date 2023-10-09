namespace TelegramBot.Application;

public static class Extensions
{
    public static string GetFirstCommand(this string str)
    {
        return string.Concat(str.Skip(1).TakeWhile(ch => !char.IsWhiteSpace(ch)));
    }
}