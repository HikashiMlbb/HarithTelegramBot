namespace TelegramBot.Application.Data.Commands.Common.AttributesAndInterfaces;

public class CommandAttribute : Attribute
{
    public CommandAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; init; }
}