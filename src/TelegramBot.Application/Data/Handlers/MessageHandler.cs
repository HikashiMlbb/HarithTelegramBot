using Serilog;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Services.Interfaces;
using TelegramBot.Application.Shared;

namespace TelegramBot.Application.Data.Handlers;

// ReSharper disable once UnusedType.Global
public class MessageHandler : IHandler
{
    private readonly ILogger _logger = Log.ForContext<MessageHandler>();
    
    private readonly CancellationToken _cancellationToken;
    private readonly ICommandExecuteService _commandExecuteService;
    private readonly IEnumerable<IMessageHandler> _messageHandlers;

    public MessageHandler(ICommandExecuteService commandExecuteService, IStoppingToken stoppingToken, IEnumerable<IMessageHandler> messageHandlers)
    {
        _commandExecuteService = commandExecuteService;
        _messageHandlers = messageHandlers;
        _cancellationToken = stoppingToken.Token;
    }

    public UpdateType UpdateType => UpdateType.Message;

    public async Task HandleAsync(Update update)
    {
        var message = update.Message!;
        // Checks if user has written a bot command at the begin of the message.
        if (message.IsCommand())
        {
            var textCommand = message.Text!.GetFirstCommand();

            var command = await _commandExecuteService.FindCommandAsync(textCommand);

            if (command is null)
            {
                _logger.Information(
                    "{user} tried to call /{command} at chat {chatId}, but it doesn't exist",
                    message.From!.ToString(), 
                    textCommand,
                    message.Chat.Id);
                return;
            }
            
            _logger.Information("{user} called /{command} at chat {chatId}", message.From!.ToString(), textCommand, message.Chat.Id);
            
            await _commandExecuteService.ExecuteCommandAsync(command, message, _cancellationToken);
        }
        else
        {
            _logger.Information(
                "{user} left a message {message} at chat {chatId}", 
                message.From!.ToString(), 
                message.Text,
                message.Chat.Id);
        }
        

        await HandleOtherAsync(update);
    }

    private async Task HandleOtherAsync(Update update)
    {
        var tasks = _messageHandlers
            .Select(messageHandler => messageHandler.Handle(update, _cancellationToken))
            .ToList();

        await Task.WhenAll(tasks);
    }
}