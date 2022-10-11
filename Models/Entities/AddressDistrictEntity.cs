using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;

namespace Monolithic.Models.Entities;

[Table(TableName.ADDRESS_DISTRICT)]
public class AddressDistrictEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [ForeignKey(nameof(AddressProvinceEntity))]
    [Column("province_id")]
    public int AddressProvinceEntityId { get; set; }

    public AddressProvinceEntity AddressProvinceEntity { get; set; }

    public ICollection<AddressWardEntity> AddressWardEntities { get; set; }
}