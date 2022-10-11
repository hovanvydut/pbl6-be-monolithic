using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Common;
using Monolithic.Models.Entities;

namespace Monolithic.Models.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (EntityEntry<EntityBase> entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.Now;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<AddressDistrictEntity> AddressDistrictEntities { get; set; }
    public DbSet<AddressProvinceEntity> AddressProvinceEntities { get; set; }
    public DbSet<AddressWardEntity> AddressWardEntities { get; set; }
    public DbSet<CategoryEntity> CategoryEntities { get; set; }
    public DbSet<PostEntity> PostEntities { get; set; }
    public DbSet<TenantTypeEntity> TenantTypeEntities { get; set; }
}
