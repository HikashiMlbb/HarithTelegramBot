using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Infrastructure.Data;

namespace TelegramBot.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BasicPartitionContext _db;

    public UnitOfWork(BasicPartitionContext db)
    {
        Members = new MembersRepository(db);
        Events = new EventsRepository(db);
        _db = db;
    }

    public IMembersRepository Members { get; init; }
    public IEventsRepository Events { get; init; }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await _db.SaveChangesAsync(cancellationToken);
    }
}