using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Monolithic.Models.Entities;

[Table(TableName.POST_PROPERTY)]
[Index(nameof(PostPropertyEntity.PostId), nameof(PostPropertyEntity.PropertyId), IsUnique = true)]
public class PostPropertyEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey(nameof(Post))]
    [Column("post_id")]
    public int PostId { get; set; }

    public PostEntity Post { get; set; }

    [ForeignKey(nameof(Property))]
    [Column("property_id")]
    public int PropertyId { get; set; }

    public PropertyEntity Property { get; set; }

    public override string ToString()
    {
        return GetType().GetProperties()
            .Select(info => (info.Name, Value: info.GetValue(this, null) ?? "(null)"))
            .Aggregate(
                new StringBuilder(),
                (sb, pair) => sb.AppendLine($"{pair.Name}: {pair.Value}"),
                sb => sb.ToString());
    }
}