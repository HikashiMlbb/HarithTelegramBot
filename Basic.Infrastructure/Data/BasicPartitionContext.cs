using Microsoft.EntityFrameworkCore;
using Basic.Domain.Entities;

namespace Basic.Infrastructure.Data;

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