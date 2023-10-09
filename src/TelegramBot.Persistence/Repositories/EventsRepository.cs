using Microsoft.EntityFrameworkCore;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Exceptions.Events;
using TelegramBot.Domain.Repositories;
using TelegramBot.Infrastructure.Data;

namespace TelegramBot.Persistence.Repositories;

public class EventsRepository : IEventsRepository
{
    private readonly BasicPartitionContext _db;

    public EventsRepository(BasicPartitionContext db)
    {
        _db = db;
    }

    public async Task<Event> AddAsync(string name, long chatId, float multiplier, CancellationToken cancellationToken)
    {
        var existEvent = await _db.Set<Event>().AsNoTracking().FirstOrDefaultAsync(@event => @event.Name == name && @event.ChatId == chatId, cancellationToken);

        if (existEvent is not null)
        {
            throw new EventAlreadyExistException(name, chatId);
        }
        
        var @event = new Event(name, chatId, multiplier);
        return (await _db.Set<Event>().AddAsync(@event, cancellationToken)).Entity;
    }

    public async Task<Event?> SingleAsync(string name, long chatId, CancellationToken cancellationToken)
    {
        return await _db.Set<Event>()
            .AsNoTracking()
            .FirstOrDefaultAsync(@event => @event.Name == name && @event.ChatId == chatId, cancellationToken);
    }

    public async Task<Event> RemoveAsync(string name, long chatId, CancellationToken cancellationToken)
    {
        var entity = await _db.Set<Event>()
            .FirstOrDefaultAsync(@event => @event.Name == name && @event.ChatId == chatId, cancellationToken);

        return entity is null ? throw new EventNotFoundException(name, chatId) : _db.Set<Event>().Remove(entity).Entity;
    }

    public async Task<IEnumerable<Event>> AllAsync(long chatId, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _db.Set<Event>().AsNoTracking().Where(@event => @event.ChatId == chatId), cancellationToken);
    }
}