namespace TelegramBot.Application.Shared;

public interface IStoppingToken
{
    public CancellationToken Token { get; set; }
}