using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBot _bot;
    private readonly IStoppingToken _stoppingToken;

    public Worker(ILogger<Worker> logger, IBot bot, IStoppingToken stoppingToken)
    {
        _logger = logger;
        _bot = bot;
        _stoppingToken = stoppingToken;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _stoppingToken.RegisterToken(stoppingToken);
        await _bot.StartAsync();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000, stoppingToken);
        }
    }
}