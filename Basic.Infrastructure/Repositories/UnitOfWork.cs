using Basic.Infrastructure.Data;
using Basic.Domain.Interfaces;

namespace Basic.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        Members = new BotMembersRepository(db);
        _db = db;
    }

    public IBotMembersRepository Members { get; init; }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await _db.SaveChangesAsync(cancellationToken);
    }
}