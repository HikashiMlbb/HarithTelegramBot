using TelegramBot.Domain.Entities;

namespace TelegramBot.Domain.Repositories;

public interface IEventsRepository
{
    public Task<Event> AddAsync(string name, long chatId, float multiplier, CancellationToken cancellationToken);
    public Task<Event?> SingleAsync(string name, long chatId, CancellationToken cancellationToken);
    public Task<Event> RemoveAsync(string name, long chatId, CancellationToken cancellationToken);
    public Task<IEnumerable<Event>> AllAsync(long chatId, CancellationToken cancellationToken);
}