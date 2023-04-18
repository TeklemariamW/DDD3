using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options)
        : base(options)
    {
    }
    public DbSet<Owner>? Owners { get; set; }
    public DbSet<Account>? Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Owner>()
            .ToContainer("Owners")
            .HasPartitionKey(ow => ow.OwnerId)
            .HasMany(ac => ac.Accounts);

        modelBuilder.Entity<Account>()
            .ToContainer("Account")
            .HasPartitionKey(ow => ow.AccountId)
            .HasOne(o => o.Owner);
    }
}
