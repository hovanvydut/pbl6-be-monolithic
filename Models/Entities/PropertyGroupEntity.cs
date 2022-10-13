using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;

namespace Monolithic.Models.Entities;

[Table(TableName.PROPERTY_GROUP)]
public class PropertyGroupEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("display_name")]
    public string DisplayName { get; set; }

    public ICollection<PropertyEntity> Properties {get; set;}
}