using Microsoft.EntityFrameworkCore;
using TelegramBot.Domain.Entities;

namespace TelegramBot.Infrastructure.Data;

public class BasicPartitionContext : DbContext
{
    public BasicPartitionContext(DbContextOptions<BasicPartitionContext> options) : base(options)
    {
    }

    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasicPartitionContext).Assembly);
    }
}