using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.USER_STATISTIC)]
public class UserStatisticEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("key")]
    public string Key { get; set; }

    [Column("value")]
    public double Value { get; set; }

    [ForeignKey(nameof(UserAccount))]
    [Column("user_id")]
    public int UserId { get; set; }
    public UserAccountEntity UserAccount { get; set; }
}