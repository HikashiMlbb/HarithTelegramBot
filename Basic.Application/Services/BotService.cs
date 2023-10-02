using System.Net;
using Basic.Application.Data.Constraints;
using Basic.Application.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Basic.Domain.Exceptions.Running;
using Basic.Infrastructure.Data.Options;

namespace Basic.Application.Services;

public class BotService : IBot
{
    private readonly ILogger _logger = Log.ForContext<BotService>();
    private ITelegramBotClient? _bot;
    private bool _isRunning;

    public BotService(IOptions<BotSettings> botOptions,
        IConfiguration config,
        IServiceProvider serviceProvider,
        IStoppingToken stoppingToken,
        IHostEnvironment hostEnvironment)
    {
        _botSettings = botOptions.Value;
        _config = config;
        _serviceProvider = serviceProvider;
        _stoppingToken = stoppingToken;
        _hostEnvironment = hostEnvironment;
    }

    public ITelegramBotClient CurrentBot => GetBot();

    public async Task StartAsync()
    {
        await Task.Run(Start);
    }

    private void Start()
    {
        if (_isRunning) return;

        var handler = _serviceProvider.GetRequiredService<IUpdateHandler>();

        CurrentBot.StartReceiving(
            handler,
            BotConstraints.ReceiverOptions,
            _stoppingToken.Token);

        _isRunning = true;
    }

    private ITelegramBotClient GetBot()
    {
        if (_bot != null) return _bot;

        var httpClientHandler = new HttpClientHandler();

        var token = _hostEnvironment.IsDevelopment() ? _config["BotToken"] : _botSettings.Token;

        if (token is null) throw new TokenIsEmptyException();

        var proxy = _botSettings.Proxy;

        if (proxy is null)
            _logger.Warning(
                "WARNING! You're not using Proxy because appsettings.json doesn't have \"Proxy\" key in \"BotSettings\" section.\n");

        httpClientHandler.Proxy = proxy is not null ? new WebProxy(proxy) : HttpClient.DefaultProxy;

        _bot = new TelegramBotClient(token, new HttpClient(httpClientHandler));
        return _bot;
    }

    #region DI Fields

    private readonly BotSettings _botSettings;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly IStoppingToken _stoppingToken;
    private readonly IHostEnvironment _hostEnvironment;

    #endregion
}