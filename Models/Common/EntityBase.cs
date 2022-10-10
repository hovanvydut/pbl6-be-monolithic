using System.ComponentModel.DataAnnotations;

namespace Monolithic.Models.Common;

public abstract class EntityBase
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}