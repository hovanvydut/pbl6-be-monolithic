namespace Monolithic.Models.DTO;

public class PostStatisticDTO
{
    public string Key { get; set; }

    public double Value { get; set; }

    public int PostId { get; set; }

    public DateTime CreatedAt { get; set; }
}