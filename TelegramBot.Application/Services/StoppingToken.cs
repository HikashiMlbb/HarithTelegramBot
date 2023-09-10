using Serilog;
using TelegramBot.Application.Data.Interfaces;

namespace TelegramBot.Application.Services;

public class StoppingToken : IStoppingToken
{
    private readonly ILogger _logger = Log.ForContext<StoppingToken>();

    private CancellationToken? _token;

    public CancellationToken Token
    {
        get => GetToken();
        set => _token ??= value;
    }

    private CancellationToken GetToken()
    {
        if (_token != null) return (CancellationToken)_token;
        _logger.Warning(
            "StoppingToken is unable to get cancellation token. Generating new token.. You should register token using RegisterToken(CancellationToken) method");
        _token = default(CancellationToken);

        return (CancellationToken)_token;
    }
}