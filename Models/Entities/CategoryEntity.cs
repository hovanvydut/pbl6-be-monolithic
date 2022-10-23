using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Monolithic.Models.Entities;

[Table(TableName.CATEGORY)]
[Index(nameof(Slug), IsUnique = true)]
public class CategoryEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("display_name")]
    public string Name { get; set; }

    [Column("slug")]
    public string Slug { get; set; }

    // public ICollection<PostEntity> Posts { get; set; }
}