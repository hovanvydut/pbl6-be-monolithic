namespace Monolithic.Models.Common;

public abstract class EntityBase
{
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}