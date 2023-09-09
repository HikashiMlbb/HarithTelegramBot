namespace TelegramBot.Application.Interfaces;

public interface IStoppingToken
{
    public void RegisterToken(CancellationToken cancellationToken);
    public CancellationToken Token { get; }
}