using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;

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

    [ForeignKey(nameof(AddressWard))]
    public int AddressWardId { get; set; }

    [Column("address_ward_id")]
    public AddressWardEntity AddressWard { get; set; }

    [ForeignKey(nameof(TenantType))]
    public int TenantTypeId { get; set; }

    [Column("tenant_type_id")]
    public TenantTypeEntity TenantType { get; set; }

    [ForeignKey(nameof(Category))]
    [Column("category_id")]
    public int CategoryId { get; set; }

    public CategoryEntity Category { get; set; }
}