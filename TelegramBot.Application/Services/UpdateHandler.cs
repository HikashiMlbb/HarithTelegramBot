using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Interfaces;

namespace TelegramBot.Application.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger _logger;
    private readonly ICommandExecutor _commandExecutor;

    public UpdateHandler(ILogger logger, ICommandExecutor commandExecutor)
    {
        _logger = logger.ForContext<UpdateHandler>();
        _commandExecutor = commandExecutor;
        
        _logger.Information("Hello, brother! {number}", 13);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        _logger.Information("I've got an update!");
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
                        _logger.Debug("Hello, world!");
                        await _commandExecutor.ExecuteCommandAsync(command, message, cancellationToken);
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
            _logger.Error("Polling exception: {error}", exception);
            throw new Exception("Polling exception");
        }, cancellationToken);
    }
}