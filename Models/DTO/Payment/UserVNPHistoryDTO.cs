namespace Monolithic.Models.DTO;

public class UserVNPHistoryDTO
{
    public string OrderInfo { get; set; }
    public long Amount { get; set; }
    public string BankCode { get; set; }
    public string TransactionStatus { get; set; }
    public int UserAccountId { get; set; }
    public string UserEmail { get; set; }
    public DateTime CreatedAt { get; set; }
}