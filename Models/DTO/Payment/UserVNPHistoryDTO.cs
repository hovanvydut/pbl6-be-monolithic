namespace Monolithic.Models.DTO;

public class UserVNPHistoryDTO
{
    public string vnp_OrderInfo { get; set; }
    public long vnp_Amount { get; set; }
    public string vnp_BankCode { get; set; }
    public string TransactionStatus { get; set; }
    public int UserAccountId { get; set; }
    public string UserEmail { get; set; }
    public DateTime CreatedAt { get; set; }
}