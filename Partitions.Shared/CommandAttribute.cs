namespace Partitions.Shared;

public class CommandAttribute : Attribute
{
    public CommandAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; init; }
}