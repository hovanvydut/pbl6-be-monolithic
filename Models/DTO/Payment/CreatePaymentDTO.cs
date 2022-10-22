namespace Monolithic.Models.DTO;
public class CreatePaymentDTO
{
    public long Amount { get; set; }
    public string BankCode { get; set; }
    public string OrderDesc { get; set; }
}