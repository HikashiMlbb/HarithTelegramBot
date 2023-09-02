namespace TelegramBot.Application.Commands.Common.AttributesAndInterfaces;

public class CommandAttribute : Attribute
{
    public string Name { get; init; }

    public CommandAttribute(string name)
    {
        Name = name;
    }
}