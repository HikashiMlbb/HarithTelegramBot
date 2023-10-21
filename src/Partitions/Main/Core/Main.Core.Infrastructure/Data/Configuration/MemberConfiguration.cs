using Main.Core.Domain.Entities;
using Main.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

#pragma warning disable CS8603 // Possible null reference return.

namespace Main.Core.Infrastructure.Data.Configuration;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
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

        builder
            .Property(m => m.Account)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Account>(v));
    }
}