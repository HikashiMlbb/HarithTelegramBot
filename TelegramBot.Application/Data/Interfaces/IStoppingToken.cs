namespace TelegramBot.Application.Data.Interfaces;

public interface IStoppingToken
{
    public void RegisterToken(CancellationToken cancellationToken);
    public CancellationToken Token { get; }
}