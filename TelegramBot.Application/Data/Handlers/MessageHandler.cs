using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Application.Data.Handlers;

// ReSharper disable once UnusedType.Global
public class MessageHandler : IHandler
{
    private readonly IBot _bot;
    private readonly ICommandExecutor _commandExecutor;

    public MessageHandler(IBot bot, ICommandExecutor commandExecutor)
    {
        _bot = bot;
        _commandExecutor = commandExecutor;
    }

    public UpdateType UpdateType => UpdateType.Message;


    public async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message!;
        // Checks if user has written a bot command at the begin of the message.
        if (!message.IsCommand()) return;

        var textCommand = message.Text!.GetFirstCommand();

        if (await _commandExecutor.FindCommandAsync(textCommand) is not { } command) return;

        await command.ExecuteAsync(message, cancellationToken);
    }
}