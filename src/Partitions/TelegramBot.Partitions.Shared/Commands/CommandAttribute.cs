namespace TelegramBot.Partitions.Shared.Commands;

public class CommandAttribute : Attribute
{
    public CommandAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; init; }
}