using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBot.Application.Data.Constraints;
using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Infrastructure.Data.Options;

namespace TelegramBot.Application.Services;

public class Bot : IBot
{
    private ITelegramBotClient? _bot;
    private readonly BotOptions _botBotOptions;
    private readonly ILogger<Bot> _logger;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly IStoppingToken _stoppingToken;
    private bool _isRunning = false;

    public Bot(IOptions<BotOptions> botOptions, 
               ILogger<Bot> logger,
               IConfiguration config, 
               IStoppingToken stoppingToken, 
               IServiceProvider serviceProvider)
    {
        _botBotOptions = botOptions.Value;
        _logger = logger;
        _config = config;
        _stoppingToken = stoppingToken;
        _serviceProvider = serviceProvider;
    }

    public ITelegramBotClient CurrentBot => GetBot();
    public async Task StartAsync()
    {
        await Task.Run(() =>
        {
            if (_isRunning)
            {
                return;
            }

            IUpdateHandler handler = _serviceProvider.GetRequiredService<IUpdateHandler>();
            
            CurrentBot.StartReceiving(
                handler,
                BotConstraints.ReceiverOptions,
                _stoppingToken.Token);

            _isRunning = true;
        });
    }

    private ITelegramBotClient GetBot()
    {
        if (_bot != null)
        {
            return _bot;
        }

        HttpClientHandler httpClientHandler = new HttpClientHandler();

        string token = _botBotOptions.Token;
        string? proxy = _botBotOptions.Proxy;

        if (proxy is null)
        {
            _logger.LogWarning(
                "WARNING! You're not using Proxy because appsettings.json doesn't have \"Proxy\" key in \"BotSettings\" section.\n");
        }

        httpClientHandler.Proxy = proxy is not null ? new WebProxy(proxy) : HttpClient.DefaultProxy;
        httpClientHandler.UseProxy = true;
        
        _bot = new TelegramBotClient(token, new HttpClient(httpClientHandler));
        return _bot;
    }
}