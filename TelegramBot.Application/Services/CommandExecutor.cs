﻿using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TelegramBot.Application.Data.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Application.Services;

public class CommandExecutor : ICommandExecutor
{
    private readonly IEnumerable<ICommonCommand> _commands;
    private readonly ILogger<CommandExecutor> _logger;

    public CommandExecutor(ILogger<CommandExecutor> logger, IEnumerable<ICommonCommand> commands)
    {
        _logger = logger;
        _commands = commands;
    }

    public async Task<ICommonCommand?> FindCommandAsync(string commandName)
    {
        return await Task.Run(() => FindCommand(commandName));
    }

    public async Task ExecuteCommandAsync(ICommonCommand command, Message message, CancellationToken cancellationToken)
    {
        await command.ExecuteAsync(message, cancellationToken);
    }

    private ICommonCommand? FindCommand(string commandName)
    {
        return _commands.FirstOrDefault(Predicate);

        bool Predicate(ICommonCommand command)
        {
            return command.GetAttribute<CommandAttribute>() is { } attribute && attribute.Name == commandName;
        }
    }
}