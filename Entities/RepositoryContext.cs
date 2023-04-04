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
            .OwnsMany(ac => ac.Accounts);

    }
}
