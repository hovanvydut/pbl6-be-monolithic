using Microsoft.EntityFrameworkCore.ChangeTracking;
using Monolithic.Models.Context.Configurations;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Extensions;

namespace Monolithic.Models.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
        modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
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
                    entry.Entity.CreatedAt = DateTime.Now.GetLocalTime();
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.Now.GetLocalTime();
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
    public DbSet<BookmarkEntity> Bookmarks { get; set; }
    public DbSet<BankCodeEntity> BankCodes { get; set; }
    public DbSet<VNPHistoryEntity> VNPHistory { get; set; }
    public DbSet<ConfigSettingEntity> ConfigSettings { get; set; }
    public DbSet<PaymentHistoryEntity> PaymentHistories { get; set; }
    public DbSet<MeetingEntity> Meetings { get; set; }
    public DbSet<FreeTimeEntity> FreeTimes { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<PriorityPostEntity> PriorityPosts { get; set; }
    public DbSet<PostStatisticEntity> PostStatistics { get; set; }
    public DbSet<UserStatisticEntity> UserStatistics { get; set; }
    public DbSet<NotificationEntity> Notifications { get; set; }
}
