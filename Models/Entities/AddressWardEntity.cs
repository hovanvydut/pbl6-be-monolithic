using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;

namespace Monolithic.Models.Entities;

[Table(TableName.ADDRESS_WARD)]
public class AddressWardEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [ForeignKey(nameof(AddressDistrictEntity))]
    [Column("district_id")]
    public int AddressDistrictEntityId { get; set; }

    public AddressDistrictEntity AddressDistrictEntity { get; set; }
}