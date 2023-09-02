using System.Text;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Services.Interfaces;

namespace TelegramBot.Application.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly ICommandExecutor _commandExecutor;

    public UpdateHandler(ILogger<UpdateHandler> logger, ICommandExecutor commandExecutor)
    {
        _logger = logger;
        _commandExecutor = commandExecutor;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (update.Type)
        {
            case UpdateType.Message:
                Message message = update.Message!;
                // Checks if user has written a bot command at the begin of the message.
                if (update.Message!.Entities is { } entities && entities.Any(IsCommand))
                {
                    string textCommand = string.Concat(message.Text!.Split(' ').First().Skip(1));
                    ICommonCommand? command = await _commandExecutor.FindCommandAsync(textCommand);
                    if (command is not null)
                    {
                        await _commandExecutor.ExecuteCommandAsync(command, botClient, message, cancellationToken);
                    }
                    break;
                }
                
                break;
                
                bool IsCommand(MessageEntity entity) => entity is { Type: MessageEntityType.BotCommand, Offset: 0 };
        }
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        await Task.Factory.StartNew(() =>
        {
            _logger.LogCritical("Polling exception: {error}", exception.Message);
            throw new Exception("Polling exception");
        }, cancellationToken);
    }
}