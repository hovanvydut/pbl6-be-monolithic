using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;

namespace Monolithic.Models.Entities;

[Table(TableName.PROPERTY)]
public class PropertyEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("display_name")]
    public string DisplayName { get; set; }

    [ForeignKey(nameof(PropertyGroup))]
    [Column("property_group_id")]
    public int PropertyGroupId {get; set;}

    public PropertyGroupEntity PropertyGroup {get; set;}
}