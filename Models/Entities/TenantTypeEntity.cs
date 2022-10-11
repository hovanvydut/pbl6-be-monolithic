using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.TENANT_TYPE)]
public class TenantTypeEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [Column("display_name")]
    public string Name { get; set; }

    [Column("slug")]
    public string Slug { get; set; }

    public ICollection<PostEntity> Posts { get; set; }
}