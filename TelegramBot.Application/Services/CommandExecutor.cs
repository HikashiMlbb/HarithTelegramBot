using System.Reflection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Services.Interfaces;

namespace TelegramBot.Application.Services;

public class CommandExecutor : ICommandExecutor
{
    private readonly ILogger<CommandExecutor> _logger;
    
    public CommandExecutor(ILogger<CommandExecutor> logger)
    {
        _logger = logger;
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
    
    public async Task<ICommonCommand?> FindCommandAsync(string commandName)
    {
        return await Task.Run(() => FindCommand(commandName));
    }

    public async Task ExecuteCommandAsync(ICommonCommand command, ITelegramBotClient bot, Message message, CancellationToken cancellationToken)
    {
        await command.ExecuteAsync(bot, message, cancellationToken);
    }
}