using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Models.Entities;

[Table(TableName.PAYMENT_HISTORY)]
[Index(nameof(PaymentHistoryEntity.PaymentCode), IsUnique = true)]
public class PaymentHistoryEntity : EntityBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("payment_code")]
    public string PaymentCode { get; set; }

    [ForeignKey(nameof(HostAccount))]
    [Column("host_id")]
    public int HostId { get; set; }
    public UserAccountEntity HostAccount { get; set; }

    [ForeignKey(nameof(Post))]
    [Column("post_id")]
    public int PostId { get; set; }
    public PostEntity Post { get; set; }

    [Column("payment_type")]
    public string PaymentType { get; set; }

    [Column("amount")]
    public double Amount { get; set; }

    [Column("description")]
    public string Description { get; set; }
}