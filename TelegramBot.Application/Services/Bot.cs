using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Domain.POCOs;

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

    public ITelegramBotClient CurrentBot
    {
        get
        {
            if (_bot != null)
            {
                return _bot;
            }

            string? token = _botBotOptions.Token;

            if (token == null)
            {
                _logger.LogCritical("Bot Configuration: Token is null.");
                throw new NullReferenceException("Bot Configuration: Token is null.");
            }

            string? proxyUri = _config["ProxyUri"];
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            
            if (proxyUri != null)
            {
                httpClientHandler.Proxy = new WebProxy(proxyUri);
                httpClientHandler.UseProxy = true;
            }
            else
            {
                _logger.LogWarning("WARNING! You're not using Proxy because appsettings.json doesn't have \"ProxyUri\" key.");
            }
            _bot = new TelegramBotClient(token, new HttpClient(httpClientHandler));
            return _bot;
        }
    }
}