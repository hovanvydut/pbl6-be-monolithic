using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.PERMISSION)]
public class PermissionEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("key")]
    public string Key { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [ForeignKey(nameof(Role))]
    [Column("role_id")]
    public int RoleId { get; set; }
    public RoleEntity Role { get; set; }
}