namespace Monolithic.Models.DTO;

public class StatisticDTO
{
    public string Key { get; set; }

    public double Value { get; set; }

    public int UserAccountId { get; set; }

    public DateTime CreatedAt { get; set; }
}