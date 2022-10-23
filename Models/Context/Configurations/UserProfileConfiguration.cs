using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;

namespace Monolithic.Models.Context.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
{
    public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.HasIndex(c => new { c.PhoneNumber, c.IdentityNumber, c.UserAccountId }).IsUnique();

        builder.HasData
        (
            new UserProfileEntity
            {
                Id = 1,
                DisplayName = "Phuoc Truong",
                PhoneNumber = "0382609982",
                IdentityNumber = "197499999",
                Avatar = "",
                CurrentCredit = 0,
                Address = "54 Nguyễn Lương Bằng",
                AddressWardId = 6351,
                UserAccountId = 1,
                CreatedAt = DateTime.Now,
            },
            new UserProfileEntity
            {
                Id = 2,
                DisplayName = "Phuong Tran",
                PhoneNumber = "0336615425",
                IdentityNumber = "012312312",
                Avatar = "",
                CurrentCredit = 0,
                Address = "54 Nguyễn Lương Bằng",
                AddressWardId = 6351,
                UserAccountId = 2,
                CreatedAt = DateTime.Now,
            },
            new UserProfileEntity
            {
                Id = 3,
                DisplayName = "Dung Nguyen",
                PhoneNumber = "0702479981",
                IdentityNumber = "12345678",
                Avatar = "",
                CurrentCredit = 0,
                Address = "54 Nguyễn Lương Bằng",
                AddressWardId = 6351,
                UserAccountId = 3,
                CreatedAt = DateTime.Now,
            },
            new UserProfileEntity
            {
                Id = 4,
                DisplayName = "Vy Ho",
                PhoneNumber = "0123456789",
                IdentityNumber = "12345678",
                Avatar = "",
                CurrentCredit = 0,
                Address = "54 Nguyễn Lương Bằng",
                AddressWardId = 6351,
                UserAccountId = 4,
                CreatedAt = DateTime.Now,
            }
        );
    }
}