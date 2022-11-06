namespace Monolithic.Models.DTO;

public class PaymentHistoryDTO
{
    public string PaymentCode { get; set; }

    public int HostId { get; set; }

    public string HostEmail { get; set; }

    public int PostId { get; set; }

    public string PaymentType { get; set; }

    public double Amount { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }
}