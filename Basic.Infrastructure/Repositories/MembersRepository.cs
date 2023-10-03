using System.Linq.Expressions;
using Basic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Basic.Domain.Entities;
using Basic.Domain.Exceptions.Members;
using Basic.Domain.Interfaces;
using Basic.Domain.ValueObjects;

namespace Basic.Infrastructure.Repositories;

public class MembersRepository : IMembersRepository
{
    private readonly BasicPartitionContext _db;

    public MembersRepository(BasicPartitionContext db)
    {
        _db = db;
    }

    public async Task<Member> AddAsync(Member member, CancellationToken cancellationToken = default)
    {
        var isMemberInDatabase = await IsMemberExistInDatabaseAsync(member, cancellationToken);
        if (isMemberInDatabase) throw new MemberAlreadyExistsException(member);

        return (await _db.AddAsync(member, cancellationToken)).Entity;
    }

    public async Task<Member?> FindUserByAccountAsync(Account account, CancellationToken cancellationToken = default)
    {
        return await _db.Set<Member>().SingleOrDefaultAsync(member => member.Account == account, cancellationToken);
    }

    public async Task<IEnumerable<Member>> FindUsersByChatIdAsync(long id,
        CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _db.Set<Member>().AsNoTracking().Where(member => member.Account.ChatId == id), cancellationToken);
    }

    public async Task<IEnumerable<Member>> FilterByAsync(Predicate<Member> predicate,
        CancellationToken cancellationToken = default)
    {
        return await Task.Run(
            () => _db.Set<Member>().AsNoTracking().Where(Expression.Lambda<Func<Member, bool>>(Expression.Call(predicate.Method))),
            cancellationToken);
    }

    public async Task<Member> RewardMember(Member member, float reward,
        CancellationToken cancellationToken = default)
    {
        var isMemberInDatabase = await IsMemberExistInDatabaseAsync(member, cancellationToken);
        if (!isMemberInDatabase) throw new MemberNotFoundException(member);

        var target = await _db.Members.SingleAsync(
            dbMember => dbMember.Account == member.Account,
            cancellationToken);

        target.Experience += reward;
        return target;
    }

    private async Task<bool> IsMemberExistInDatabaseAsync(Member targetMember,
        CancellationToken cancellationToken = default)
    {
        return await _db.Set<Member>().AnyAsync(dbMember => dbMember.Account == targetMember.Account, cancellationToken);
    }
}