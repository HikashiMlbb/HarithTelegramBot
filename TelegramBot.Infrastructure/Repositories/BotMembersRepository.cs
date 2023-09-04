using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Exceptions.Members;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Infrastructure.Data;

namespace TelegramBot.Infrastructure.Repositories;

public class BotMembersRepository : IBotMembersRepository
{
    private readonly ILogger<BotMembersRepository> _logger;
    private readonly ApplicationDbContext _db;
    
    public BotMembersRepository(ILogger<BotMembersRepository> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }
    
    public async Task<BotMember> AddAsync(BotMember member, CancellationToken cancellationToken = default)
    {
        bool isMemberInDatabase = await IsMemberExistInDatabaseAsync(member, cancellationToken);
        if (isMemberInDatabase)
        {
            throw new MemberAlreadyExistsException(member);
        }

        EntityEntry<BotMember> trackingEntity = await _db.AddAsync(member, cancellationToken);
        return trackingEntity.Entity;

    }

    public async Task<BotMember?> FindUserByTelegramAndChatIdAsync(long telegramId, long chatId, CancellationToken cancellationToken = default)
    {
        return await _db.Members.SingleOrDefaultAsync(member => member.TelegramId == telegramId && member.ChatId == chatId, cancellationToken);
    }

    public async Task<IEnumerable<BotMember>> FindUsersByChatIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _db.Members.Where(member => member.ChatId == id), cancellationToken);
    }

    public async Task<IEnumerable<BotMember>> FilterByAsync(Predicate<BotMember> predicate, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _db.Members.Where(Expression.Lambda<Func<BotMember, bool>>(Expression.Call(predicate.Method))), cancellationToken);
    }

    public async Task<BotMember> RewardMember(BotMember member, float reward, CancellationToken cancellationToken = default)
    {
        bool isMemberInDatabase = await IsMemberExistInDatabaseAsync(member, cancellationToken);
        if (!isMemberInDatabase)
        {
            throw new MemberNotFoundException(member);
        }

        BotMember target = await _db.Members.SingleAsync(
            dbMember => dbMember.TelegramId == member.TelegramId && dbMember.ChatId == member.ChatId,
            cancellationToken);

        target.Experience += reward;
        return target;
    }

    private async Task<bool> IsMemberExistInDatabaseAsync(BotMember targetMember, CancellationToken cancellationToken = default)
    {
        return await _db.Members.AnyAsync(dbMember => dbMember.TelegramId == targetMember.TelegramId && dbMember.ChatId == targetMember.ChatId, cancellationToken);
    }
}