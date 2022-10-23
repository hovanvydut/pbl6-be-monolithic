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

        var defaultPassword = "123456";
        var passwordSecure = PasswordSecure.GetPasswordHash(defaultPassword);
        var passwordHashed = passwordSecure.PasswordHashed;
        var passwordSalt = passwordSecure.PasswordSalt;
        builder.HasData
        (
            new UserAccountEntity
            {
                Id = 1,
                Email = "ht10082001@gmail.com",
                PasswordHashed = passwordHashed,
                PasswordSalt = passwordSalt,
                IsVerified = true,
                RoleId = 1,
                CreatedAt = DateTime.Now,
            },
            new UserAccountEntity
            {
                Id = 2,
                Email = "tranphiphuong2763@gmail.com",
                PasswordHashed = passwordHashed,
                PasswordSalt = passwordSalt,
                IsVerified = true,
                RoleId = 1,
                CreatedAt = DateTime.Now,
            },
            new UserAccountEntity
            {
                Id = 3,
                Email = "dungngminh1311@gmail.com",
                PasswordHashed = passwordHashed,
                PasswordSalt = passwordSalt,
                IsVerified = true,
                RoleId = 1,
                CreatedAt = DateTime.Now,
            },
            new UserAccountEntity
            {
                Id = 4,
                Email = "hovanvydut@gmail.com",
                PasswordHashed = passwordHashed,
                PasswordSalt = passwordSalt,
                IsVerified = true,
                RoleId = 1,
                CreatedAt = DateTime.Now,
            }
        );
    }
}