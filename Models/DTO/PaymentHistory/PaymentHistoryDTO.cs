namespace Monolithic.Models.DTO;

public class PaymentHistoryDTO
{
    public string PaymentCode { get; set; }

    public int UserId { get; set; }

    public string UserEmail { get; set; }

    public int PostId { get; set; }

    public string PaymentType { get; set; }

    public double Amount { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }
}