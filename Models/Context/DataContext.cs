using Microsoft.EntityFrameworkCore.ChangeTracking;
using Monolithic.Models.Context.Configurations;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;

namespace Monolithic.Models.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
        modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
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
    public DbSet<UserAccountEntity> UserAccounts { get; set; }
    public DbSet<UserProfileEntity> UserProfiles { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<PermissionEntity> Permissions { get; set; }
    public DbSet<PropertyEntity> Properties { get; set; }
    public DbSet<PropertyGroupEntity> PropertyGroups { get; set; }
    public DbSet<PostPropertyEntity> PostProperties { get; set; }
    public DbSet<MediaEntity> Medias { get; set; }
    public DbSet<BankCodeEntity> BankCodes { get; set; }
    public DbSet<VNPHistoryEntity> VNPHistory { get; set; }
}
