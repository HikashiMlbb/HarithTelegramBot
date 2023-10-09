using System.Reflection;
using Serilog;
using Telegram.Bot.Types;
using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Application.Data.Shared;
using ILogger = Serilog.ILogger;

namespace TelegramBot.Application.Services;

public class CommandExecuteService : ICommandExecuteService
{
    private readonly ILogger _logger = Log.ForContext<CommandExecuteService>();
    
    private readonly IEnumerable<ITextCommand> _commands;
    private readonly IServiceProvider _serviceProvider;
    

    public CommandExecuteService(IEnumerable<ITextCommand> commands, IServiceProvider serviceProvider)
    {
        _commands = commands;
        _serviceProvider = serviceProvider;
    }

    public async Task<ITextCommand?> FindCommandAsync(string commandName)
    {
        return await Task.Run(() => FindCommand(commandName));
    }

    public async Task ExecuteCommandAsync(ITextCommand commandHandler, Message message, CancellationToken cancellationToken)
    {
        await Task.Run(() => ExecuteCommand(commandHandler, message, cancellationToken), cancellationToken);
    }

    private Task ExecuteCommand(ITextCommand commandHandler, Message message, CancellationToken cancellationToken)
    {
        var implementType = typeof(ITextCommandHandler<>).MakeGenericType(commandHandler.GetType());
        var handler = _serviceProvider.GetService(implementType);

        if (handler is null)
        {
            _logger.Information("Command handler not found!");
            return Task.CompletedTask;
        }

        var executeAsyncMethod = handler.GetType()
            .GetMethod("ExecuteAsync", new[] { typeof(Message), typeof(CancellationToken) });

        if (executeAsyncMethod is null)
        {
            _logger.Information("ExecuteAsync not found!");
            return Task.CompletedTask;
        }

        executeAsyncMethod.Invoke(handler, new object[] { message, cancellationToken });
        return Task.CompletedTask;
    }

    #region Private Methods

    private ITextCommand? FindCommand(string commandName)
    {
        return (
            from command in _commands 
            let result = command.GetType().GetCustomAttribute<CommandAttribute>()
            where result?.Name == commandName 
            select command).FirstOrDefault();
    }

    #endregion
    
}