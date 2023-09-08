using TelegramBot.Domain.Interfaces;
using TelegramBot.Infrastructure.Data;

namespace TelegramBot.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IBotMembersRepository Members { get; init; }

    public UnitOfWork(IBotMembersRepository members, ApplicationDbContext db)
    {
        Members = members;
        _db = db;
    }
    
    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await _db.SaveChangesAsync(cancellationToken);
    }
}