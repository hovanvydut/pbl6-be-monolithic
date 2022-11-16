namespace Monolithic.Models.DTO;

public class PostStatisticDTO
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public bool IsDeleted { get; set; }

    public double StatisticValue { get; set; }
}