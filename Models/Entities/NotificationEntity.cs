using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.NOTIFICATION)]
public class NotificationEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    public string Code { get; set; }

    [Column("extra_data")]
    public string ExtraData { get; set; }

    [Column("has_read")]
    public bool HasRead { get; set; }

    [ForeignKey(nameof(OriginUserAccount))]
    [Column("origin_user_id")]
    public int OriginUserId { get; set; }
    public UserAccountEntity OriginUserAccount { get; set; }

    [ForeignKey(nameof(TargetUserAccount))]
    [Column("target_user_id")]
    public int TargetUserId { get; set; }
    public UserAccountEntity TargetUserAccount { get; set; }
}