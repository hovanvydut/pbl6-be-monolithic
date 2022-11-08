namespace Monolithic.Models.DTO;

public class PriorityPostDTO
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public string Address { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}