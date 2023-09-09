using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Data.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Data.Constraints;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Application.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger _logger;
    private readonly ICommandExecutor _commandExecutor;
    private readonly IEnumerable<IHandler> _handlers;

    public UpdateHandler(ILogger logger, ICommandExecutor commandExecutor, IServiceProvider serviceProvider)
    {
        _logger = logger.ForContext<UpdateHandler>();
        _commandExecutor = commandExecutor;

        _handlers = serviceProvider.GetServices<IHandler>();
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (!BotConstraints.ReceiverOptions.AllowedUpdates!.Contains(update.Type))
        {
            return;
        }

        IHandler? handler = _handlers.FirstOrDefault(handler => handler.UpdateType == update.Type);

        if (handler is null)
        {
            return;
        }

        await handler.HandleAsync(update, cancellationToken);
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