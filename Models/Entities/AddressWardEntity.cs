using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.ADDRESS_WARD)]
public class AddressWardEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [ForeignKey(nameof(AddressDistrict))]
    [Column("district_id")]
    public int AddressDistrictId { get; set; }

    public AddressDistrictEntity AddressDistrict { get; set; }
}