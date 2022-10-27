using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.BOOKMARK)]
public class BookmarkEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey(nameof(Post))]
    [Column("post_id")]
    public int PostId { get; set; }
    public PostEntity Post { get; set; }

    [ForeignKey(nameof(GuestAccount))]
    [Column("guest_id")]
    public int GuestId { get; set; }
    public UserAccountEntity GuestAccount { get; set; }
}