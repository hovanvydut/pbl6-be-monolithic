using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Monolithic.Models.Entities;

[Table(TableName.FREE_TIME)]
public class FreeTimeEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey(nameof(UserAccount))]
    [Column("user_id")]
    public int UserId { get; set; }

    public UserAccountEntity UserAccount { get; set; }

    [Column("start")]
    public string Start { get; set; }

    [Column("end")]
    public string End { get; set; }

    [Column("day")]
    public int Day { get; set; }
}