using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.POST_STATISTIC)]
public class PostStatisticEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("key")]
    public string Key { get; set; }

    [Column("value")]
    public double Value { get; set; }

    [ForeignKey(nameof(Post))]
    [Column("post_id")]
    public int PostId { get; set; }
    public PostEntity Post { get; set; }
}