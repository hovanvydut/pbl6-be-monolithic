namespace Monolithic.Models.DTO;
public class PaymentInfoDTO
{
    public long OrderId { get; set; }
    public long Amount { get; set; }
    public string OrderDesc { get; set; }

    public DateTime CreatedDate { get; set; }
    public string Status { get; set; }

    public long PaymentTranId { get; set; }
    public string BankCode { get; set; }
    public string PayStatus { get; set; }
}