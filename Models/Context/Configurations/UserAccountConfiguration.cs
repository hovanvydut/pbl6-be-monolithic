using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;
using Monolithic.Helpers;

namespace Monolithic.Models.Context.Configurations;

public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccountEntity>
{
    public void Configure(EntityTypeBuilder<UserAccountEntity> builder)
    {
        builder.HasIndex(c => new { c.Email }).IsUnique();
    }
}