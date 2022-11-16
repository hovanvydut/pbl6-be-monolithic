namespace Monolithic.Models.DTO;

public class PostStatisticGroupDTO
{
    public IEnumerable<PostStatisticDTO> Posts { get; set; }

    public double StatisticValue { get; set; }

    public string StatisticDate { get; set; }
}