using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Services;
using TelegramBot.Application.Services.Interfaces;

namespace TelegramBot.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBot _bot;
    private readonly IStoppingToken _stoppingToken;
    private readonly IUpdateHandler _updateHandler;

    public Worker(ILogger<Worker> logger, IBot bot, IStoppingToken stoppingToken, IUpdateHandler updateHandler)
    {
        _logger = logger;
        _bot = bot;
        _stoppingToken = stoppingToken;
        _updateHandler = updateHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ITelegramBotClient bot = _bot.CurrentBot;
        _stoppingToken.RegisterToken(stoppingToken);

        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] { UpdateType.Message, UpdateType.ChatMember, UpdateType.Poll, UpdateType.CallbackQuery },
            Limit = 10,
            ThrowPendingUpdates = true
        };

        bot.StartReceiving(_updateHandler, receiverOptions, stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000, stoppingToken);
        }
    }
}