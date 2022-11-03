using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Monolithic.Models.Entities;

[Table(TableName.FREE_TIME)]
[Index(nameof(UserId), nameof(Hour), nameof(Day), IsUnique = true)]
public class FreeTimeEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey(nameof(UserAccount))]
    [Column("user_id")]
    public int UserId {get; set;}
    
    public UserAccountEntity UserAccount {get; set;}

    [Column("hour")]
    public int Hour {get; set;}

    [Column("day")]
    public int Day {get; set;}
}