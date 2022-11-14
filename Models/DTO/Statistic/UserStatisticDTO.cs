namespace Monolithic.Models.DTO;

public class UserStatisticDTO
{
    public string Key { get; set; }

    public double Value { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }
}