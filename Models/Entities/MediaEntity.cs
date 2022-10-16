using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;

namespace Monolithic.Models.Entities;

[Table(TableName.MEDIA)]
[Index(nameof(EntityId))]
public class MediaEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [Column("content_type")]
    public string ContentType { get; set; }

    [Column("url")]
    public string Url { get; set; }

    [Column("entity_type")]
    public EntityType EntityType {get; set;}

    [Column("entity_id")]
    public int EntityId {get; set;}
}