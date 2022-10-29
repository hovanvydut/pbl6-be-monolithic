using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.CONFIG_SETTING)]
[Index(nameof(ConfigSettingEntity.Key), IsUnique = true)]
public class ConfigSettingEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("key")]
    public string Key { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("value")]
    public double Value { get; set; }
}