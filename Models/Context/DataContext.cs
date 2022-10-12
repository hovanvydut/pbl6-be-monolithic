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

    public DbSet<AddressDistrictEntity> AddressDistricts { get; set; }
    public DbSet<AddressProvinceEntity> AddressProvinces { get; set; }
    public DbSet<AddressWardEntity> AddressWards { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<TenantTypeEntity> TenantTypes { get; set; }
    public DbSet<UserAccountEntity> UserAccounts { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<PermissionEntity> Permissions { get; set; }
}
