using Serilog;
using TelegramBot.Application.Shared;
using ILogger = Serilog.ILogger;

namespace TelegramBot.Worker;

public class Worker : BackgroundService
{
    private readonly IBotService _botService;
    private readonly IStoppingToken _stoppingToken;
    private readonly ILogger _logger;

    public Worker(IBotService botService, IStoppingToken stoppingToken)
    {
        _logger = Log.ForContext<Worker>();
        _botService = botService;
        _stoppingToken = stoppingToken;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _stoppingToken.Token = stoppingToken;
        try
        {
            await _botService.StartAsync();
        }
        catch (Exception e)
        {
            _logger.Fatal("Fatality in Worker service: {err}", e.ToString());
            return;
        }

        while (!stoppingToken.IsCancellationRequested) await Task.Delay(10000, stoppingToken);
    }
}