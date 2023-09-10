using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Exceptions.Members;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Domain.ValueObjects;
using TelegramBot.Infrastructure.Data;

namespace TelegramBot.Infrastructure.Repositories;

public class BotMembersRepository : IBotMembersRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<BotMembersRepository> _logger;

    public BotMembersRepository(ILogger<BotMembersRepository> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<BotMember> AddAsync(BotMember member, CancellationToken cancellationToken = default)
    {
        var isMemberInDatabase = await IsMemberExistInDatabaseAsync(member, cancellationToken);
        if (isMemberInDatabase) throw new MemberAlreadyExistsException(member);

        return (await _db.AddAsync(member, cancellationToken)).Entity;
    }

    public async Task<BotMember?> FindUserByAccountAsync(Account account, CancellationToken cancellationToken = default)
    {
        return await _db.Members.SingleOrDefaultAsync(member => member.Account == account, cancellationToken);
    }

    public async Task<IEnumerable<BotMember>> FindUsersByChatIdAsync(long id,
        CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _db.Members.Where(member => member.Account.ChatId == id), cancellationToken);
    }

    public async Task<IEnumerable<BotMember>> FilterByAsync(Predicate<BotMember> predicate,
        CancellationToken cancellationToken = default)
    {
        return await Task.Run(
            () => _db.Members.Where(Expression.Lambda<Func<BotMember, bool>>(Expression.Call(predicate.Method))),
            cancellationToken);
    }

    public async Task<BotMember> RewardMember(BotMember member, float reward,
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

    private async Task<bool> IsMemberExistInDatabaseAsync(BotMember targetMember,
        CancellationToken cancellationToken = default)
    {
        return await _db.Members.AnyAsync(dbMember => dbMember.Account == targetMember.Account, cancellationToken);
    }
}