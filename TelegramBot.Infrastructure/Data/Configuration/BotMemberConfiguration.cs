using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TelegramBot.Domain.Entities;
using TelegramBot.Domain.ValueObjects;

#pragma warning disable CS8603 // Possible null reference return.

namespace TelegramBot.Infrastructure.Data.Configuration;

public class BotMemberConfiguration : IEntityTypeConfiguration<BotMember>
{
    public void Configure(EntityTypeBuilder<BotMember> builder)
    {
        builder
            .HasKey("Id");
        builder
            .Property(member => member.Account)
            .HasConversion(
                account => JsonConvert.SerializeObject(account),
                account => JsonConvert.DeserializeObject<Account>(account));
    }
}