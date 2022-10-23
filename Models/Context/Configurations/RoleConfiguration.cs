using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;

namespace Monolithic.Models.Context.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasIndex(c => new { c.Name }).IsUnique();

        builder.HasData
        (
            new RoleEntity
            {
                Id = 1,
                Name = "admin",
                Description = "Quản lý",
                CreatedAt = DateTime.Now
            },
            new RoleEntity
            {
                Id = 2,
                Name = "host",
                Description = "Chủ trọ",
                CreatedAt = DateTime.Now
            },
            new RoleEntity
            {
                Id = 3,
                Name = "guest",
                Description = "Khách tìm trọ",
                CreatedAt = DateTime.Now
            }
        );
    }
}