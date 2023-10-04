namespace TelegramBot.Domain.Interfaces;

public interface IUnitOfWork
{
    public IMembersRepository Members { get; init; }
    public IEventsRepository Events { get; init; }

    public Task<int> CompleteAsync(CancellationToken cancellationToken);
}