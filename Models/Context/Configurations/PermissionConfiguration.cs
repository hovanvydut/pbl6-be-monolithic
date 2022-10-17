using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;

namespace Monolithic.Models.Context.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.HasIndex(c => new { c.Key }).IsUnique();
    }
}