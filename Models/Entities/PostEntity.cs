using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Monolithic.Models.Entities;

[Table(TableName.POST)]
[Index(nameof(PostEntity.Slug), IsUnique = true)]
public class PostEntity : EntityBase
{
    [Key]
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

    [Column("address_ward_id")]
    [ForeignKey(nameof(AddressWard))]
    public int AddressWardId { get; set; }

    public AddressWardEntity AddressWard { get; set; }

    [ForeignKey(nameof(Category))]
    [Column("category_id")]
    public int CategoryId { get; set; }

    public CategoryEntity Category { get; set; }

    [ForeignKey(nameof(HostAccount))]
    [Column("host_id")]
    public int HostId { get; set; }

    public UserAccountEntity HostAccount { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public ICollection<PostPropertyEntity> PostProperties { get; set; }

    public override string ToString()
    {
        return GetType().GetProperties()
            .Select(info => (info.Name, Value: info.GetValue(this, null) ?? "(null)"))
            .Aggregate(
                new StringBuilder(),
                (sb, pair) => sb.AppendLine($"{pair.Name}: {pair.Value}"),
                sb => sb.ToString());
    }
}