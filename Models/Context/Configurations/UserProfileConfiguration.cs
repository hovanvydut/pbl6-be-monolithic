using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;

namespace Monolithic.Models.Context.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
{
    public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.HasIndex(c => new { c.PhoneNumber, c.IdentityNumber, c.UserAccountId }).IsUnique();
    }
}