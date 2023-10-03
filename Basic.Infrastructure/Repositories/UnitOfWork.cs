using Basic.Domain.Entities;
using Basic.Infrastructure.Data;
using Basic.Domain.Interfaces;

namespace Basic.Infrastructure.Repositories;

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