using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;
using Monolithic.Constants;

namespace Monolithic.Models.Context.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        // builder.HasIndex(c => new { c.Key }).IsUnique();

        // Seed permission for admin role
        // var allPermission = PermissionPolicy.AllPermissions;
        // var adminPermissions = allPermission.Select((per, idx) => new PermissionEntity
        // {
        //     Id = idx + 1,
        //     Key = per.Key,
        //     Description = per.Description,
        //     RoleId = 1,
        //     CreatedAt = DateTime.Now
        // }).ToArray();
        // builder.HasData(adminPermissions);
    }
}