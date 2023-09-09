using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.Application.Interfaces;
using TelegramBot.Infrastructure.Data.Options;

namespace TelegramBot.Application.Services;

public class Bot : IBot
{
    private ITelegramBotClient? _bot;
    private readonly BotOptions _botBotOptions;
    private readonly ILogger<Bot> _logger;
    private readonly IConfiguration _config;

    public Bot(IOptions<BotOptions> botOptions, ILogger<Bot> logger, IConfiguration config)
    {
        _botBotOptions = botOptions.Value;
        _logger = logger;
        _config = config;
    }

    public ITelegramBotClient CurrentBot => GetBot();

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