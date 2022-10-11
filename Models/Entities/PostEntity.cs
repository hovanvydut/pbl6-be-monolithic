using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;

namespace Monolithic.Models.Entities;

[Table(TableName.POST)]
public class PostEntity : EntityBase
{
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("area")]
    public float Area { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [Column("price")]
    public float Price { get; set; }

    [Column("pre_paid_price")]
    public float PrePaidPrice { get; set; }

    [Column("slug")]
    public string Slug { get; set; }

    [Column("limit_tenant")]
    public int LimitTenant { get; set; }

    [Column("num_view")]
    public int NumView { get; set; }

    [ForeignKey(nameof(AddressWardEntity))]
    public int AddressWardEntityId { get; set; }

    [Column("address_ward_id")]
    public AddressWardEntity AddressWardEntity { get; set; }

    [ForeignKey(nameof(TenantTypeEntity))]
    public int TenantTypeEntityId { get; set; }

    [Column("tenant_type_id")]
    public TenantTypeEntity TenantTypeEntity { get; set; }

    [ForeignKey(nameof(CategoryEntity))]
    [Column("category_id")]
    public int CategoryEntityId { get; set; }

    public CategoryEntity CategoryEntity { get; set; }
}