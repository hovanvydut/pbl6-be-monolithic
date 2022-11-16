namespace Monolithic.Models.DTO;

public class UserStatisticGroupDTO
{
    public IEnumerable<UserStatisticDTO> Users { get; set; }

    public double StatisticValue { get; set; }

    public string StatisticDate { get; set; }
}