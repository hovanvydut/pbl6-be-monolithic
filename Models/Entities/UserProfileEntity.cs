using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.USER_PROFILE)]
public class UserProfileEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("display_name")]
    public string DisplayName { get; set; }

    [Column("phone_number")]
    public string PhoneNumber { get; set; }

    [Column("identity_number")]
    public string IdentityNumber { get; set; }

    [Column("current_credit")]
    public double CurrentCredit { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [ForeignKey(nameof(AddressWard))]
    [Column("address_ward_id")]
    public int AddressWardId { get; set; }
    public AddressWardEntity AddressWard { get; set; }

    [ForeignKey(nameof(UserAccount))]
    [Column("user_id")]
    public int UserAccountId { get; set; }
    public UserAccountEntity UserAccount { get; set; }
}