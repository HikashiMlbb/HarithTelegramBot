using System.Reflection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Data.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Application.Services;

public class CommandExecutor : ICommandExecutor
{
    private readonly ILogger<CommandExecutor> _logger;
    private readonly IEnumerable<ICommonCommand> _commands;
    
    public CommandExecutor(ILogger<CommandExecutor> logger, IEnumerable<ICommonCommand> commands)
    {
        _logger = logger;
        _commands = commands;
    }

    private ICommonCommand? FindCommand(string commandName)
    {
        Type interfaceType = typeof(ICommonCommand);

        IEnumerable<Type> commands = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => interfaceType.IsAssignableFrom(type) && type.IsClass);

        foreach (Type command in commands)
        {
            CommandAttribute? attribute = command.GetCustomAttribute<CommandAttribute>();
            if (attribute is null)
            {
                continue;
            }

            if (attribute.Name != commandName)
            {
                continue;
            }

            ICommonCommand commonCommand = (ICommonCommand)Activator.CreateInstance(command)!;
            CommandAttribute commandAttribute = commonCommand.GetType().GetCustomAttribute<CommandAttribute>()!;

            return commonCommand;
        }

        return null;
    }

    private ICommonCommand? FindCommandNew(string commandName)
    {
        foreach (ICommonCommand command in _commands)
        {
            if (command.GetType().GetCustomAttribute<CommandAttribute>() is not { } attr)
            {
                continue;
            }

            if (attr.Name != commandName)
            {
                continue;
            }

            return command;
        }

        return null;
    }
    
    public async Task<ICommonCommand?> FindCommandAsync(string commandName)
    {
        return await Task.Run(() => FindCommandNew(commandName));
    }

    public async Task ExecuteCommandAsync(ICommonCommand command, Message message, CancellationToken cancellationToken)
    {
        await command.ExecuteAsync(message, cancellationToken);
    }
}