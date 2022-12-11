using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.REVIEW)]
public class ReviewEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey(nameof(Post))]
    [Column("post_id")]
    public int PostId { get; set; }

    public PostEntity Post { get; set; }

    [ForeignKey(nameof(UserAccount))]
    [Column("user_id")]
    public int UserId { get; set; }

    public UserAccountEntity UserAccount { get; set; }

    [Column("content")]
    public string Content { get; set; }

    [Column("rating")]
    public int Rating { get; set; }

    [Column("sentiment")]
    public string Sentiment { get; set; }
}