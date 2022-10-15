using System.ComponentModel.DataAnnotations.Schema;

namespace Monolithic.Models.Common;

public abstract class EntityBase
{

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}