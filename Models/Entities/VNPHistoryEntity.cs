using System.ComponentModel.DataAnnotations.Schema;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Monolithic.Models.Entities;

[Table(TableName.VNP_HISTORY)]
public class VNPHistoryEntity : EntityBase
{
    [Key]
    [Column("vnp_txn_ref")]
    public long vnp_TxnRef { get; set; }

    [Column("vnp_order_info")]
    public string vnp_OrderInfo { get; set; }

    [Column("vnp_amount")]
    public long vnp_Amount { get; set; }

    [Column("vnp_bank_code")]
    public string vnp_BankCode { get; set; }

    [Column("vnp_bank_tran_no")]
    public string vnp_BankTranNo { get; set; }

    [Column("vnp_card_type")]
    public string vnp_CardType { get; set; }

    [Column("vnp_pay_date")]
    public long vnp_PayDate { get; set; }

    [Column("vnp_response_code")]
    public string vnp_ResponseCode { get; set; }

    [Column("vnp_tmn_code")]
    public string vnp_TmnCode { get; set; }

    [Column("vnp_transaction_no")]
    public string vnp_TransactionNo { get; set; }

    [Column("vnp_transaction_status")]
    public string vnp_TransactionStatus { get; set; }

    [Column("vnp_secure_hash")]
    public string vnp_SecureHash { get; set; }

    [ForeignKey(nameof(UserAccount))]
    [Column("user_id")]
    public int UserAccountId { get; set; }
    public UserAccountEntity UserAccount { get; set; }    
}