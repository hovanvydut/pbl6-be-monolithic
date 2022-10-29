using System;
using System.Collections.Generic;
using System.Web;

namespace Monolithic.Models.DTO;
public class VNPayReturnDTO
{
    public long vnp_Amount { get; set; }
    public string vnp_BankCode { get; set; }
    public string vnp_BankTranNo { get; set; }
    public string vnp_CardType { get; set; }
    public string vnp_OrderInfo { get; set; }
    public long vnp_PayDate { get; set; }
    public string vnp_ResponseCode { get; set; }
    public string vnp_TmnCode { get; set; }
    public string vnp_TransactionNo { get; set; }
    public string vnp_TransactionStatus { get; set; }
    public long vnp_TxnRef { get; set; }
    public string vnp_SecureHash { get; set; }
}