using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Basic.Domain.Entities;
using Basic.Domain.ValueObjects;

#pragma warning disable CS8603 // Possible null reference return.

namespace Basic.Infrastructure.Data.Configuration;

public class BotMemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");
        builder.HasKey(m => m.Id);

        builder.HasKey(m => m.Id);

        builder
            .Property<string>(m => m.FirstName)
            .HasColumnName("First name")
            .IsRequired();

        builder
            .Property(m => m.Level)
            .HasColumnName("Current Level")
            .HasDefaultValue(0);

        builder
            .Property(m => m.Experience)
            .HasColumnName("Current experience")
            .HasDefaultValue(0);

        /*builder.OwnsOne(m => m.Account, a =>
        {
            a.WithOwner();

            a.Property(account => account.TelegramId)
                .HasColumnName("TelegramId")
                .IsRequired();
            a.Property(account => account.ChatId)
                .HasColumnName("ChatId")
                .IsRequired();
        });*/

        builder
            .Property(m => m.Account)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Account>(v));
    }
}