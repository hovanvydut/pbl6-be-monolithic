using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.ROLE)]
public class RoleEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    public ICollection<UserAccountEntity> UserAccounts { get; set; }

    public ICollection<PermissionEntity> Permissions { get; set; }

    public RoleEntity()
    {
        UserAccounts = new HashSet<UserAccountEntity>();
        Permissions = new HashSet<PermissionEntity>();
    }
}