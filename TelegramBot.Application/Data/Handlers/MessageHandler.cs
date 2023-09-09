using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Data.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Application.Data.Handlers;

public class MessageHandler : IHandler
{
    public UpdateType UpdateType { get; } = UpdateType.Message;
    
    private readonly IBot _bot;
    private readonly ICommandExecutor _commandExecutor;

    public MessageHandler(IBot bot, ICommandExecutor commandExecutor)
    {
        _bot = bot;
        _commandExecutor = commandExecutor;
    }


    public async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        Message message = update.Message!;
        // Checks if user has written a bot command at the begin of the message.
        if (message.Entities is { } entities && entities.Any(IsCommand))
        {
            string textCommand = message.Text!.GetFirstCommand();
            
            if (await _commandExecutor.FindCommandAsync(textCommand) is not { } command)
            {
                return;
            }

            await command.ExecuteAsync(message, cancellationToken);
        }
    }
    private bool IsCommand(MessageEntity entity) => entity is { Type: MessageEntityType.BotCommand, Offset: 0 };
}