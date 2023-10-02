namespace Basic.Application.Data.Interfaces;

public interface IStoppingToken
{
    public CancellationToken Token { get; set; }
}