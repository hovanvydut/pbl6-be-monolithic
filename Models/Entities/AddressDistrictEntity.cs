using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.ADDRESS_DISTRICT)]
public class AddressDistrictEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [ForeignKey(nameof(AddressProvince))]
    [Column("province_id")]
    public int AddressProvinceId { get; set; }

    public AddressProvinceEntity AddressProvince { get; set; }

    public ICollection<AddressWardEntity> AddressWards { get; set; }
}