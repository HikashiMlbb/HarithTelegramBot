using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Domain.Primitives;

namespace TelegramBot.Domain.ValueObjects;

[Serializable]
public class Account : ValueObject, IEquatable<Account>
{
    public long TelegramId { get; }
    public long ChatId { get; }

    public Account()
    {
        TelegramId = 0;
        ChatId = 0;
    }
    public Account(long telegramId, long chatId)
    {
        TelegramId = telegramId;
        ChatId = chatId;
    }

    public bool Equals(Account? other)
    {
        return TelegramId == other?.TelegramId && ChatId == other.ChatId;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TelegramId;
        yield return ChatId;
    }

    public override bool Equals(object? obj)
    {
        return obj is Account other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TelegramId, ChatId);
    }

    public static bool operator ==(Account left, Account right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Account left, Account right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return $"{TelegramId}:{ChatId}";
    }
}