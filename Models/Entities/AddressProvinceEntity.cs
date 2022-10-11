using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;

namespace Monolithic.Models.Entities;

[Table(TableName.ADDRESS_PROVINCE)]
public class AddressProvinceEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    public ICollection<AddressDistrictEntity> AddressDistrictEntities { get; set; }
}