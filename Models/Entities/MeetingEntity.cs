using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Monolithic.Models.Entities;

[Table(TableName.MEETING)]
public class MeetingEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey(nameof(GuestAccount))]
    [Column("guest_id")]
    public int GuestId { get; set; }

    public UserAccountEntity GuestAccount { get; set; }

    [ForeignKey(nameof(Post))]
    [Column("post_id")]
    public int PostId { get; set; }

    public PostEntity Post { get; set; }

    [Column("time")]
    public DateTime Time { get; set; }

    [Column("approve_time")]
    public DateTime? ApproveTime { get; set; }

    [Column("met")]
    public bool Met { get; set; }
}