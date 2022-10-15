using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.USER_ACCOUNT)]
public class UserAccountEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("password_hashed")]
    public byte[] PasswordHashed { get; set; }

    [Column("password_salt")]
    public byte[] PasswordSalt { get; set; }

    [Column("is_verified")]
    public bool IsVerified { get; set; }

    [ForeignKey(nameof(Role))]
    [Column("role_id")]
    public int RoleId { get; set; }
    public RoleEntity Role { get; set; }

    public UserProfileEntity UserProfile { get; set; }
}