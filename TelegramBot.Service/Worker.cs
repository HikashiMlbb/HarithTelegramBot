using Serilog;
using TelegramBot.Application.Data.Interfaces;
using ILogger = Serilog.ILogger;

namespace TelegramBot.Service;

public class Worker : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IBot _bot;
    private readonly IStoppingToken _stoppingToken;

    public Worker(IBot bot, IStoppingToken stoppingToken)
    {
        _logger = Log.ForContext<Worker>();
        _bot = bot;
        _stoppingToken = stoppingToken;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _stoppingToken.RegisterToken(stoppingToken);
        try
        {
            await _bot.StartAsync();
        }
        catch (Exception e)
        {
            _logger.Fatal("Fatality in Worker service: {err}", e.ToString());
            return;
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000, stoppingToken);
        }
    }
}