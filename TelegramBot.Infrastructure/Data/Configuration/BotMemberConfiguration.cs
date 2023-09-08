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

        builder.OwnsOne(m => m.Account, a =>
        {
            a.WithOwner();
            
            a.Property(account => account.TelegramId)
                .HasColumnName("TelegramId")
                .IsRequired();
            a.Property(account => account.ChatId)
                .HasColumnName("ChatId")
                .IsRequired();
        });
    }
}