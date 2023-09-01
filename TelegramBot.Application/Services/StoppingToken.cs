using Microsoft.Extensions.Logging;
using TelegramBot.Domain.Interfaces;

namespace TelegramBot.Application.Services;

public class StoppingToken : IStoppingToken
{
    private readonly ILogger<StoppingToken> _logger;
    private CancellationToken? _token;
    public CancellationToken Token
    {
        get
        {
            if (_token == null)
            {
                _logger.LogError("StoppingToken is unable to get cancellation token. Generating new token.. You should register token using RegisterToken(CancellationToken) method");
                _token = default(CancellationToken);
            }

            return (CancellationToken)_token;
        }
        private set => _token = value;
    }

    public StoppingToken(ILogger<StoppingToken> logger)
    {
        _logger = logger;
    }
    
    public void RegisterToken(CancellationToken cancellationToken)
    {
        Token = cancellationToken;
    }
}